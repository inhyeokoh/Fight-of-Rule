using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Google.Protobuf;
using Cysharp.Threading.Tasks;

public class PacketHandlerImpl : MonoBehaviour
{
    internal static bool Handle_S_SIGNUP(Session session, S_SIGNUP message)
    {
        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorDup)
        {
#if UNITY_EDITOR
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                Utils.Log("이미 존재하는 아이디");
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistID);
            });
            return false;
#endif
        }
        
        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorExist)
        {
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                Utils.Log("이미 가입된 회원입니다");
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistUser);
            });
            return false;
        }

        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            Utils.Log("성공적으로 가입 되었습니다");
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
            GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.SignUpSuccess);
        });
        return true;
    }

    internal static bool Handle_S_LOGIN(Session session, S_LOGIN message)
    {
        if (false == message.LoginSuccess)
        {
            //경우에 따라서 재로그인 유도 (지금은 그냥 return)
            Debug.Log("로그인 실패");
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.LoginFail);
            });
            return false;
        }

        Utils.Log(message.Ip);
        Utils.Log(message.Port);
        Utils.Log(message.Uid);
        Utils.Log(message.Token);

        if (message.Ip.Length <= 0 || message.Port > 65535 || message.Port <= 0)
        {
            //이건 명확히 재시도 해야함
            //todo
            Utils.Log("로그인 실패");
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.LoginFail);
            });
            return false;
        }

        GameManager.Network.Connect(message.Ip, message.Port, NetState.NEED_VRF, new Vrf() { ip = message.Ip, port = message.Port, token = message.Token, unique_id = message.Uid });
        return true;
    }

    internal static bool Handle_S_ASK_VERF(Session session, S_ASK_VERF message)
    {
        return true;
    }

    internal static bool Handle_S_VERIFYING(Session session, S_VERIFYING message)
    {
        if (message.Sucess == false)
        {
            //TODO 게임 종료시키기
            Utils.Log("cannot verifying");
            return false;
        }

        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Login);
        });
        // 신규 유저        
        if (message.Character.Count == 0)
        {
            GameManager.Data.SelectedSlotNum = 0; // 0번 슬롯 생성하도록

            // 캐릭터 생성씬 이동
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                var loadAsync = SceneManager.LoadSceneAsync("Create");
                GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
            });
            return true;
        }

        // 기존 유저
        foreach (var charInfo in message.Character)
        {
            GameManager.Data.characters[charInfo.BaseInfo.SlotIndex] = charInfo;
        }

        // 캐릭터 선택씬 이동
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("Select");
            GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
        });

/*        GameManager.ThreadPool.UniAsyncJob(async () =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("Select");
            while (loadAsync.progress < 0.9f)
                await UniTask.Yield(PlayerLoopTiming.Update);
        });*/

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        // 생성 불가
        if (message.Success == false)
        {            
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistNickName);
            });
            return false;
        }

        // 해당 닉네임 생성 가능하면
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.Data.CurrentCharacter.BaseInfo.Nickname = ByteString.CopyFrom(GameManager.UI.InputName.nickname, System.Text.Encoding.Unicode);
            GameManager.UI.InputName.childPopups.Add(GameManager.UI.ConfirmYN);
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmYN);
            GameManager.UI.ConfirmYN.ChangeText(UI_ConfirmYN.Enum_ConfirmTypes.AskDecidingNickName);            
        });
        return true;
    }

    internal static bool Handle_S_CHARACTERS(Session session, S_CHARACTERS message)
    {
        return true;
    }

    internal static bool Handle_S_NEW_CHARACTER(Session session, S_NEW_CHARACTER message)
    {
        // 캐릭터 생성 불가 시
        if (message.Success == false)
        {
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistNickName);
            });
            return false;
        }

        // 캐릭터 생성 가능 시
        GameManager.Data.characters[GameManager.Data.SelectedSlotNum] = message.Character;
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("Select");
            GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
        });

        return true;
    }

    internal static bool Handle_S_DELETE_CHARACTER(Session session, S_DELETE_CHARACTER message)
    {
        Debug.Log("캐릭터 삭제 패킷 수신");

        if (message.Success == false)
        {
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.CharacterDeleteFail);
            });
            return false;
        }
        // 캐릭터 데이터 삭제
        GameManager.Data.characters[message.SlotNum] = null;
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
            GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.CharacterDeleteSuccess);
        });       

        return true;
    }

    internal static bool Handle_S_REQUEST_SETTINGS_OPTIONS(Session session, S_REQUEST_SETTINGS_OPTIONS message)
    {
        // 성공 여부?

        //서버로부터 받아온 환경설정 정보들을 메모리에 올리기 
        GameManager.Data.volOptions.MasterVol = message.SettingsOptions.VolOptions.MasterVol;
        GameManager.Data.volOptions.BgmVol = message.SettingsOptions.VolOptions.BgmVol;
        GameManager.Data.volOptions.EffectVol = message.SettingsOptions.VolOptions.EffectVol;
        GameManager.Data.volOptions.VoiceVol = message.SettingsOptions.VolOptions.VoiceVol;

        GameManager.Data.volOptions.MasterVolOn = message.SettingsOptions.VolOptions.MasterVolOn;
        GameManager.Data.volOptions.BgmVolOn = message.SettingsOptions.VolOptions.BgmVolOn;
        GameManager.Data.volOptions.EffectVolOn = message.SettingsOptions.VolOptions.EffectVolOn;
        GameManager.Data.volOptions.VoiceVolOn = message.SettingsOptions.VolOptions.VoiceVolOn;

        return true;
    }

    internal static bool Handle_S_SAVE_VOL_OPTIONS(Session session, S_SAVE_VOL_OPTIONS message)
    {
        return true;
    }

    internal static bool Handle_S_CHARACTER_MOVE(Session session, S_CHARACTER_MOVE message)
    {
        float x = message.CurrentPos.X / 1000000.0f;
        float y = message.CurrentPos.Y / 1000000.0f;
        float z = message.CurrentPos.Z / 1000000.0f;

        Vector3 correctionPos = new Vector3(x, y, z);

        PlayerController.instance._playerMovement.playerTransform.position = correctionPos;

        return true;
    }

    internal static bool Handle_S_INGAME(Session session, S_INGAME message)
    {
        Debug.Log($"인게임 데이터 수신 성공 여부 {message.Success}");
        if (message.Success == false)
        {
            return false;
        }

        ///////////////// 캐릭터 위치 정보 //////////////////
        Debug.Log(message.CharacterPosition);
        Debug.Log(message.CharacterPosition.X);
        Debug.Log(message.CharacterPosition.Y);
        Debug.Log(message.CharacterPosition.Z);
/*        float x = message.CharacterPosition.X / 1000000.0f;
        float y = message.CharacterPosition.Y / 1000000.0f;
        float z = message.CharacterPosition.Z / 1000000.0f;

        Vector3 currentPos = new Vector3(x, y, z);


        print($"서버에서 보내는 currentPos : {currentPos}");
        PlayerController.instance._playerMovement.playerTransform.position = currentPos;*/

        ///////////////// 인벤토리 정보 //////////////////

        return true;
    }

    internal static bool Handle_S_ITEMINFO(Session session, S_ITEMINFO message)
    {
        return true;
    }

    internal static bool Handle_S_ITEM_PICKUP(Session session, S_ITEM_PICKUP message)
    {
        return true;
    }
    internal static bool Handle_S_TEMP_MONSTER_KILL(Session session, S_TEMP_MONSTER_KILL s_TEMP_MONSTER_KILL)
    {
        return true;
    }

    internal static bool Handle_S_DROP_ITEM(Session session, S_DROP_ITEM message)
    {
        Debug.Log("Handle_S_DROP_ITEM");
        if (message.Add == true)
        {
            for (int i = 0; i < message.Item.Count; i++)
            {
                var oneOfItem = message.Item[i].ItemDataInfoCase;

                if (oneOfItem == ITEM_DATA_INFO_WITH_POS.ItemDataInfoOneofCase.EtcItem)
                {
                    var s_itemData = message.Item[i].EtcItem.ItemData;

                    var c_itemData = GameManager.Data.itemDatas[s_itemData.Id];
                    var attributes = s_itemData.Attributes;
                    var itemType = GameManager.Data.GetItemType(attributes);
                    var convertedItemType = GameManager.Data.ConvertItemType(itemType);
                    var itemGrade = GameManager.Data.GetItemGrade(attributes);
                    var convertedItemGrade = GameManager.Data.ConvertItemGrade(itemGrade);
                    var dynData = message.Item[i].EtcItem.DynItemData;

                    ItemData item = new ItemData(s_itemData.Id, c_itemData.name, c_itemData.desc, c_itemData.icon, convertedItemType, convertedItemGrade, s_itemData.SellingPrice, s_itemData.MaxCount, dynData.Count, dynData.SlotNum);
                    GameManager.Data.dropTestItems.Add(item);
                    GameManager.Data.dropTestItemsPos.Add(new Vector3(message.Item[i].Pos.X, message.Item[i].Pos.Y, message.Item[i].Pos.Z));
                }
                else if (oneOfItem == ITEM_DATA_INFO_WITH_POS.ItemDataInfoOneofCase.ConsumeItem)
                {
                    var s_stateItemData = message.Item[i].ConsumeItem.ConsumeItemData.StateitemData;
                    var s_itemData = s_stateItemData.ItemData;

                    var c_itemData = GameManager.Data.itemDatas[s_itemData.Id];
                    var attributes = s_itemData.Attributes;
                    var itemType = GameManager.Data.GetItemType(attributes);
                    var convertedItemType = GameManager.Data.ConvertItemType(itemType);
                    var itemGrade = GameManager.Data.GetItemGrade(attributes);
                    var convertedItemGrade = GameManager.Data.ConvertItemGrade(itemGrade);
                    var dynData = message.Item[i].ConsumeItem.DynItemData;

                    var detailType = GameManager.Data.GetConsumptionDetailType(attributes);
                    var convertedDeatailType = GameManager.Data.ConvertConsumptionDetailType(detailType);
                                        
                    ConsumptionItemData item = new ConsumptionItemData(s_itemData.Id, c_itemData.name, c_itemData.desc, c_itemData.icon, convertedItemGrade, convertedItemType, convertedDeatailType, s_itemData.SellingPrice,
                        s_stateItemData.Level, s_stateItemData.AttackBoost, s_stateItemData.DefenseBoost, s_stateItemData.SpeedBoost, s_stateItemData.AttackSpeedBoost, s_stateItemData.HpRecovery, s_stateItemData.ExpBoost,
                        s_stateItemData.MaxHpBoost, s_stateItemData.MaxHpBoost, s_stateItemData.MpRecovery, s_stateItemData.DurationBool, s_stateItemData.Duration, s_itemData.MaxCount, dynData.Count, dynData.SlotNum);

                    GameManager.Data.dropTestItems.Add(item);
                    GameManager.Data.dropTestItemsPos.Add(new Vector3(message.Item[i].Pos.X, message.Item[i].Pos.Y, message.Item[i].Pos.Z));
                }
                else if (oneOfItem == ITEM_DATA_INFO_WITH_POS.ItemDataInfoOneofCase.EquipItem)
                {
                    var s_stateItemData = message.Item[i].EquipItem.EquipItemData.StateitemData;
                    var s_itemData = s_stateItemData.ItemData;
                    var c_itemData = GameManager.Data.itemDatas[s_itemData.Id];

                    var attributes = s_itemData.Attributes;
                    var itemType = GameManager.Data.GetItemType(attributes);
                    var convertedItemType = GameManager.Data.ConvertItemType(itemType);
                    var itemGrade = GameManager.Data.GetItemGrade(attributes);
                    var convertedItemGrade = GameManager.Data.ConvertItemGrade(itemGrade);
                    var detailType = GameManager.Data.GetEquipmentDetailType(attributes);
                    var convertedDeatailType = GameManager.Data.ConvertEquipmentDetailType(detailType);
                    var classType = GameManager.Data.GetItemClass(attributes);
                    var convertedClassType = GameManager.Data.ConvertItemClass(classType);
                    var dynData = message.Item[i].EquipItem.DynItemData;
                    
                    int maxReinforce = message.Item[i].EquipItem.EquipItemData.MaxReinforcement;
                    int curReinforce = message.Item[i].EquipItem.ReinforceItemData.CurrentReinforcement;

                    EquipmentItemData item = new EquipmentItemData(s_itemData.Id, c_itemData.name, c_itemData.desc, c_itemData.icon, convertedClassType, convertedItemGrade, convertedItemType, convertedDeatailType, s_itemData.SellingPrice,
                        s_stateItemData.Level, s_stateItemData.AttackBoost, s_stateItemData.DefenseBoost, s_stateItemData.SpeedBoost, s_stateItemData.AttackSpeedBoost, s_stateItemData.HpRecovery, s_stateItemData.ExpBoost,
                        s_stateItemData.MaxHpBoost, s_stateItemData.MaxHpBoost, s_stateItemData.MpRecovery, maxReinforce, s_stateItemData.DurationBool, s_stateItemData.Duration, s_itemData.MaxCount, dynData.Count, dynData.SlotNum, curReinforce);
                    GameManager.Data.dropTestItems.Add(item);
                    GameManager.Data.dropTestItemsPos.Add(new Vector3(message.Item[i].Pos.X, message.Item[i].Pos.Y, message.Item[i].Pos.Z));
                }
            }
        }


        // TODO 아이템 삭제
        return true;
    }

    internal static bool Handle_S_SYNC_PLAYER(Session session, S_SYNC_PLAYER s_SYNC_PLAYER)
    {
        Debug.Log("Handle_S_SYNC_PLAYER");
        return true;
    }
}
