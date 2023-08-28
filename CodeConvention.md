# 말머리
1. 가독성을 최우선으로 삼는다.
2. 정말 합당한 이유가 있지 않는 한, 통합개발환경(IDE)의 자동 서식을 따른다. (비주얼 스튜디오의 "Ctrl + K + D" 기능)
---

# I. 메인 코딩 표준
<details>
  <summary>펼치기</summary>

  **1. 클래스, 구조체의 이름은 파스칼 표기법을 따른다.**
  ``` C++
  class  CardManager;
  struct CardData;
  ```
  <br>

  **2. 지역 변수, 함수의 매개 변수, 클래스의 필드 맴버의 이름은 카멜 표기법을 따른며, 그 역할이 분명하도록 이름을 지정한다.**
  ``` C#
  public void SomeMethod(int someParameter)
  {
      int someNumber;
      int id;
  }

  public class Widget
  {
      float width;
      float height;
  }
  ```
  <br>

  **3. 프로퍼티의 경우 파스칼 표기법을 따르며, public이 아닌 경우 프로퍼티 사용을 지양한다.**
  ```C#
  public class Knight
  {
    int _state;
    public int State { get { return _state; } };

    public int TmpCount { get; set; }
  }

  //아래는 지양해야할 프로퍼티 사용 방식
  private NetFlowObject nfo { get; set;} // => private object _nfo; 로 대체 가능
  ```
  <br>


  **4. 메서드,함수 이름은 파스칼 표기법을 따르며, 기본적으로 동사(명령형)+명사(목적어)의 형태로 만드나, 그렇지 않은 경우 최대한 직관적인 함수명을 사용한다.**
  ``` C#
  public int GetAge()
  {
      // 함수 구현부...
  }
  ```
  <br>


  **5. 최대한 직관적인 함수명을 사용하되, 그렇지 않은 경우 반드시 아래와 같이 함수용도와 매개변수, 반환 타입에 대한 설명을 명시한다.(vs studio 기준 '/'세번 입력시 자동완성)**
  ``` C#
  /*=================================
        직관적인 이름으로 설정
  =================================*/
  public bool IsValid();
  public int GetCount();

  
  /*=========================================
        비직관적이거나, 특수한 기능을 가짐
  ==========================================*/
  /// <summary>
  /// Params    => buffer = 버퍼, offset = 버퍼 시작 길이, len = 버퍼 길이
  /// ret       => bool: 부착 성공/실패 여부
  /// </summary>
  /// <param name="buffer"></param>
  /// <param name="offset"></param>
  /// <param name="len"></param>
  public bool AttachBufferToIO(ArraySegment<byte> buffer, int offset, int len);

  //혹은

  /// <summary>
  /// ret       => bool: 부착 성공/실패 여부
  /// </summary>
  /// <param name="buffer">버퍼</param>
  /// <param name="offset">버퍼 시작 길이</param>
  /// <param name="len">버퍼 길이</param>
  public bool AttachBufferToIO(ArraySegment<byte> buffer, int offset, int len);
  ```
  <br>

  **6. public 필드, 메서드가 아닌 경우엔 이름 앞에 _(Underscore/Under-bar/언더바)를 붙인다.(내부에서만 사용하겠다는 뜻)**
  ``` C#
  private uint _GetAge()
  {
      // 함수 구현부...
  }

  public class PathFinder
  {
    int _maxSequence;
    private List<Vector3> _prevDirections;
  }
  ```
<br>

  **7. ~~상수의 이름은 모두 대문자로 하되 밑줄로 각 단어를 분리한다.~~**
  > 기본적으로 필요한 경우가 아니면 사용을 금한다.
  ``` C#
  const int SOME_CONSTANT = 1;
  ```
<br>

  **8. 상수로 사용하는 개체형 변수에는 static readonly를 사용하고, 변수는 모두 대문자로 하되 밑줄로 각 단어를 분리한다.**
  ``` C#
  public static readonly MyConstClass MY_CONST_OBJECT = new MyConstClass();
  ```
  <br>


  **9. 초기화 후 값이 변하지 않는 변수는 readonly로 선언한다.**
  ``` C#
  public class Account
  {
      private readonly string mPassword;

      public Account(string password)
      {
          mPassword = password;
      }
  }
  ```
  <br>


  **10. 네임스페이스의 이름은 파스칼 표기법을 따른다.**
  ``` C#
  namespace System.Graphics
  ```
  <br>


  **11. 인터페이스를 선언할 때는 앞에 I를 붙인다.**
  ``` C#
  interface ISomeInterface;
  ```
  <br>


  **12. 열거형을 선언할 때는 앞에 Enum_를 붙인다**
  ``` C#
  public enum Enum_Direction
  {
      North,
      South
  }
  ```
  <br>


  **13.  재귀 함수는 Recursive_를 붙인다.**
  ``` C#
  public void Recursive_Fibonacci();
  ```
  <br>


  **14. 제네릭 매서드는 Generic_을 붙인다**
  ``` C#
  public void Generic_ChangeState<T>(string name, T value)
  {
      //TODO
  }
  ```
  <br>


  **15. 코루틴 전용 메서드를 선언할 경우 앞에 Co_를 붙인다.**
  ```C#
  IEnumerator Co_SwitchEffect()
  {
      //TODO
  }
  ```
  <br>

  **16. 그 밖에 동일한 상속, 유사한 기능 등으로 구성되는 여러 클래스명 및 파일의 경우 접두에 명시하고, 동일한 폴더로 관리한다.**
  > 상속을 사용하는 경우
  ```C#
  public abstract class Weapon
  {
      //...
  }

  public class Weapon_Bow : Weapon
  {
      //...
  }

  public class Weapon_Sword : Weapon
  {
      //...
  }

  public class Weapon_Bow_BasicBow : Weapon_Bow
  {
      //...
  }
  ```

  > 유사한 기능의 경우
  ```C#
  //오브젝트 동기화용
  public class NetFlow_Object
  {
      //...
  }
  //플레이어 동기화용
  public class NetFlow_Player
  {
      //...
  }
  ```
<br>

  **17. 뒤에 추가적인 단어가 오지 않는 경우 줄임말은 모두 대문자로 표기한다.**
  ``` C#
  public int OrderID { get; private set; }
  public int HttpCode { get; private set; }
  ```
  <br>


  **18. getter와 setter 대신 프로퍼티를 사용한다.(C#한정)**
  > 틀린 방식:
  ``` C#
  public class Employee
  {
      private string mName;
      public string GetName();
      public string SetName(string name);
  }
  ```
  <br>

  > 올바른 방식:
  ``` C#
  public class Employee
  {
      public string Name { get; set; }
  }
  ```
  <br>


  **19. 필요한 경우가 아니라면 foreach문 대신 for문을 사용한다.(단, 가독성을 심각하게 저해하는 경우 foreach를 사용해도 무방하다.)**
  ```C#
  List<Vector3> _path = new List<Vector3>();

  //...
  ```
  <br>

  > for문
  ```C#
  for(int i = 0; i < _path.Count; i++)
  {
    //TODO
  }
  ```
  <br>

  > foreach문 사용
  ```C#
  foreach(var pos in _path)
  {
    //TODO
  }
  ```
  <br>

  **20. switch 문에 언제나 default: 케이스를 넣는다.**
  ``` C#
  switch (number)
  {
      case 0:
          ... 
          break;
      default:
          break;
  }
  ```
  <br>


  **21. switch 문에서 default: 케이스가 절대 실행될 일이 없는 경우, default: 안에 Debug.Assert(false) 란 코드를 추가한다.**
  ``` C#
  switch (type)
  {
      case 1:
          ... 
          break;
      default:
  #if UNITY_EDITOR
          Debug.Assert(type <= 1);
  #endif
          break;
  }
  ```
  <br>

  > 단 Debug와 같은 Unity Editor 전용 기능을 사용할 때는 반드시 아래와 같이 사용한다.
  ```C#
  try
  {
    //TODO
  }
  catch
  {
  #if UNITY_EDITOR
      Debug.Log("Test Failed at aaa.bbb() in xxx.cs");
  #endif
  }
  ```
  <br>

  
  **22. 명시적인 캐스팅이 필요할 경우, C스타일의 캐스팅은 지양한다.**
  
  > C스타일 캐스팅
  ```C#

  public SwapBack(Controller_Object ct)
  {
    Controller controllerBody = (Controller)ct;
  }
  ```
  <br>

  > C# 캐스팅
  ```C#
  public SwapBack(Controller_Object ct)
  {
    Controller controllerBody = ct as Controller;
  }
  ```
  <br>

  **23.  ~~여러 파일이 하나의 클래스를 이룰 때(즉, partial 클래스), 파일 이름은 클래스 이름으로 시작하고, 그 뒤에 마침표와 세부 항목 이름을 붙인다.~~**
  > 필요한 경우가 아니라면 사용을 지양한다.
  ``` C#
  public partial class Human;
  ```
  ``` C#
  Human.Head.cs
  Human.Body.cs
  Human.Arm.cs
  ```
  
  </details>
  
  
  
  # II. 소스 코드 포맷팅
<details>
  <summary>펼치기</summary>

  **1. 중괄호( { )를 열 때는 언제나 새로운 줄에 연다.**
  **2. 중괄호 안( { } )에 코드가 한 줄만 있더라도 반드시 중괄호를 사용한다.**
  ``` C#
  if (bSomething)
  {
    return;
  }
  ```
  
  
  **3. 한 줄에 변수 하나만 선언한다.**
  > 틀린 방식:
  ``` C#
  int counter = 0, index = 0;
  ```
  
  > 올바른 방식:
  ``` C#
  int counter = 0;
  int index = 0;
  ```
  
  
  **4. 한 개의 명령문(;)이 길어지면 줄바꿈**
  
  </details>
