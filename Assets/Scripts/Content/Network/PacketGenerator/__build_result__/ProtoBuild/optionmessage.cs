// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: OptionMessage.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from OptionMessage.proto</summary>
public static partial class OptionMessageReflection {

  #region Descriptor
  /// <summary>File descriptor for OptionMessage.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static OptionMessageReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChNPcHRpb25NZXNzYWdlLnByb3RvIjUKEFNFVFRJTkdTX09QVElPTlMSIQoL",
          "dm9sX29wdGlvbnMYASABKAsyDC5WT0xfT1BUSU9OUyKxAQoLVk9MX09QVElP",
          "TlMSEgoKbWFzdGVyX3ZvbBgBIAEoAhIPCgdiZ21fdm9sGAIgASgCEhIKCmVm",
          "ZmVjdF92b2wYAyABKAISEQoJdm9pY2Vfdm9sGAQgASgCEhUKDW1hc3Rlcl92",
          "b2xfb24YBSABKAgSEgoKYmdtX3ZvbF9vbhgGIAEoCBIVCg1lZmZlY3Rfdm9s",
          "X29uGAcgASgIEhQKDHZvaWNlX3ZvbF9vbhgIIAEoCGIGcHJvdG8z"));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::SETTINGS_OPTIONS), global::SETTINGS_OPTIONS.Parser, new[]{ "VolOptions" }, null, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::VOL_OPTIONS), global::VOL_OPTIONS.Parser, new[]{ "MasterVol", "BgmVol", "EffectVol", "VoiceVol", "MasterVolOn", "BgmVolOn", "EffectVolOn", "VoiceVolOn" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class SETTINGS_OPTIONS : pb::IMessage<SETTINGS_OPTIONS>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<SETTINGS_OPTIONS> _parser = new pb::MessageParser<SETTINGS_OPTIONS>(() => new SETTINGS_OPTIONS());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<SETTINGS_OPTIONS> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::OptionMessageReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public SETTINGS_OPTIONS() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public SETTINGS_OPTIONS(SETTINGS_OPTIONS other) : this() {
    volOptions_ = other.volOptions_ != null ? other.volOptions_.Clone() : null;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public SETTINGS_OPTIONS Clone() {
    return new SETTINGS_OPTIONS(this);
  }

  /// <summary>Field number for the "vol_options" field.</summary>
  public const int VolOptionsFieldNumber = 1;
  private global::VOL_OPTIONS volOptions_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public global::VOL_OPTIONS VolOptions {
    get { return volOptions_; }
    set {
      volOptions_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as SETTINGS_OPTIONS);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(SETTINGS_OPTIONS other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (!object.Equals(VolOptions, other.VolOptions)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (volOptions_ != null) hash ^= VolOptions.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (volOptions_ != null) {
      output.WriteRawTag(10);
      output.WriteMessage(VolOptions);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (volOptions_ != null) {
      output.WriteRawTag(10);
      output.WriteMessage(VolOptions);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (volOptions_ != null) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(VolOptions);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(SETTINGS_OPTIONS other) {
    if (other == null) {
      return;
    }
    if (other.volOptions_ != null) {
      if (volOptions_ == null) {
        VolOptions = new global::VOL_OPTIONS();
      }
      VolOptions.MergeFrom(other.VolOptions);
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          if (volOptions_ == null) {
            VolOptions = new global::VOL_OPTIONS();
          }
          input.ReadMessage(VolOptions);
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 10: {
          if (volOptions_ == null) {
            VolOptions = new global::VOL_OPTIONS();
          }
          input.ReadMessage(VolOptions);
          break;
        }
      }
    }
  }
  #endif

}

public sealed partial class VOL_OPTIONS : pb::IMessage<VOL_OPTIONS>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<VOL_OPTIONS> _parser = new pb::MessageParser<VOL_OPTIONS>(() => new VOL_OPTIONS());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<VOL_OPTIONS> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::OptionMessageReflection.Descriptor.MessageTypes[1]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public VOL_OPTIONS() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public VOL_OPTIONS(VOL_OPTIONS other) : this() {
    masterVol_ = other.masterVol_;
    bgmVol_ = other.bgmVol_;
    effectVol_ = other.effectVol_;
    voiceVol_ = other.voiceVol_;
    masterVolOn_ = other.masterVolOn_;
    bgmVolOn_ = other.bgmVolOn_;
    effectVolOn_ = other.effectVolOn_;
    voiceVolOn_ = other.voiceVolOn_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public VOL_OPTIONS Clone() {
    return new VOL_OPTIONS(this);
  }

  /// <summary>Field number for the "master_vol" field.</summary>
  public const int MasterVolFieldNumber = 1;
  private float masterVol_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public float MasterVol {
    get { return masterVol_; }
    set {
      masterVol_ = value;
    }
  }

  /// <summary>Field number for the "bgm_vol" field.</summary>
  public const int BgmVolFieldNumber = 2;
  private float bgmVol_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public float BgmVol {
    get { return bgmVol_; }
    set {
      bgmVol_ = value;
    }
  }

  /// <summary>Field number for the "effect_vol" field.</summary>
  public const int EffectVolFieldNumber = 3;
  private float effectVol_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public float EffectVol {
    get { return effectVol_; }
    set {
      effectVol_ = value;
    }
  }

  /// <summary>Field number for the "voice_vol" field.</summary>
  public const int VoiceVolFieldNumber = 4;
  private float voiceVol_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public float VoiceVol {
    get { return voiceVol_; }
    set {
      voiceVol_ = value;
    }
  }

  /// <summary>Field number for the "master_vol_on" field.</summary>
  public const int MasterVolOnFieldNumber = 5;
  private bool masterVolOn_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool MasterVolOn {
    get { return masterVolOn_; }
    set {
      masterVolOn_ = value;
    }
  }

  /// <summary>Field number for the "bgm_vol_on" field.</summary>
  public const int BgmVolOnFieldNumber = 6;
  private bool bgmVolOn_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool BgmVolOn {
    get { return bgmVolOn_; }
    set {
      bgmVolOn_ = value;
    }
  }

  /// <summary>Field number for the "effect_vol_on" field.</summary>
  public const int EffectVolOnFieldNumber = 7;
  private bool effectVolOn_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool EffectVolOn {
    get { return effectVolOn_; }
    set {
      effectVolOn_ = value;
    }
  }

  /// <summary>Field number for the "voice_vol_on" field.</summary>
  public const int VoiceVolOnFieldNumber = 8;
  private bool voiceVolOn_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool VoiceVolOn {
    get { return voiceVolOn_; }
    set {
      voiceVolOn_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as VOL_OPTIONS);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(VOL_OPTIONS other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(MasterVol, other.MasterVol)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(BgmVol, other.BgmVol)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(EffectVol, other.EffectVol)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(VoiceVol, other.VoiceVol)) return false;
    if (MasterVolOn != other.MasterVolOn) return false;
    if (BgmVolOn != other.BgmVolOn) return false;
    if (EffectVolOn != other.EffectVolOn) return false;
    if (VoiceVolOn != other.VoiceVolOn) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (MasterVol != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(MasterVol);
    if (BgmVol != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(BgmVol);
    if (EffectVol != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(EffectVol);
    if (VoiceVol != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(VoiceVol);
    if (MasterVolOn != false) hash ^= MasterVolOn.GetHashCode();
    if (BgmVolOn != false) hash ^= BgmVolOn.GetHashCode();
    if (EffectVolOn != false) hash ^= EffectVolOn.GetHashCode();
    if (VoiceVolOn != false) hash ^= VoiceVolOn.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (MasterVol != 0F) {
      output.WriteRawTag(13);
      output.WriteFloat(MasterVol);
    }
    if (BgmVol != 0F) {
      output.WriteRawTag(21);
      output.WriteFloat(BgmVol);
    }
    if (EffectVol != 0F) {
      output.WriteRawTag(29);
      output.WriteFloat(EffectVol);
    }
    if (VoiceVol != 0F) {
      output.WriteRawTag(37);
      output.WriteFloat(VoiceVol);
    }
    if (MasterVolOn != false) {
      output.WriteRawTag(40);
      output.WriteBool(MasterVolOn);
    }
    if (BgmVolOn != false) {
      output.WriteRawTag(48);
      output.WriteBool(BgmVolOn);
    }
    if (EffectVolOn != false) {
      output.WriteRawTag(56);
      output.WriteBool(EffectVolOn);
    }
    if (VoiceVolOn != false) {
      output.WriteRawTag(64);
      output.WriteBool(VoiceVolOn);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (MasterVol != 0F) {
      output.WriteRawTag(13);
      output.WriteFloat(MasterVol);
    }
    if (BgmVol != 0F) {
      output.WriteRawTag(21);
      output.WriteFloat(BgmVol);
    }
    if (EffectVol != 0F) {
      output.WriteRawTag(29);
      output.WriteFloat(EffectVol);
    }
    if (VoiceVol != 0F) {
      output.WriteRawTag(37);
      output.WriteFloat(VoiceVol);
    }
    if (MasterVolOn != false) {
      output.WriteRawTag(40);
      output.WriteBool(MasterVolOn);
    }
    if (BgmVolOn != false) {
      output.WriteRawTag(48);
      output.WriteBool(BgmVolOn);
    }
    if (EffectVolOn != false) {
      output.WriteRawTag(56);
      output.WriteBool(EffectVolOn);
    }
    if (VoiceVolOn != false) {
      output.WriteRawTag(64);
      output.WriteBool(VoiceVolOn);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (MasterVol != 0F) {
      size += 1 + 4;
    }
    if (BgmVol != 0F) {
      size += 1 + 4;
    }
    if (EffectVol != 0F) {
      size += 1 + 4;
    }
    if (VoiceVol != 0F) {
      size += 1 + 4;
    }
    if (MasterVolOn != false) {
      size += 1 + 1;
    }
    if (BgmVolOn != false) {
      size += 1 + 1;
    }
    if (EffectVolOn != false) {
      size += 1 + 1;
    }
    if (VoiceVolOn != false) {
      size += 1 + 1;
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(VOL_OPTIONS other) {
    if (other == null) {
      return;
    }
    if (other.MasterVol != 0F) {
      MasterVol = other.MasterVol;
    }
    if (other.BgmVol != 0F) {
      BgmVol = other.BgmVol;
    }
    if (other.EffectVol != 0F) {
      EffectVol = other.EffectVol;
    }
    if (other.VoiceVol != 0F) {
      VoiceVol = other.VoiceVol;
    }
    if (other.MasterVolOn != false) {
      MasterVolOn = other.MasterVolOn;
    }
    if (other.BgmVolOn != false) {
      BgmVolOn = other.BgmVolOn;
    }
    if (other.EffectVolOn != false) {
      EffectVolOn = other.EffectVolOn;
    }
    if (other.VoiceVolOn != false) {
      VoiceVolOn = other.VoiceVolOn;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 13: {
          MasterVol = input.ReadFloat();
          break;
        }
        case 21: {
          BgmVol = input.ReadFloat();
          break;
        }
        case 29: {
          EffectVol = input.ReadFloat();
          break;
        }
        case 37: {
          VoiceVol = input.ReadFloat();
          break;
        }
        case 40: {
          MasterVolOn = input.ReadBool();
          break;
        }
        case 48: {
          BgmVolOn = input.ReadBool();
          break;
        }
        case 56: {
          EffectVolOn = input.ReadBool();
          break;
        }
        case 64: {
          VoiceVolOn = input.ReadBool();
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 13: {
          MasterVol = input.ReadFloat();
          break;
        }
        case 21: {
          BgmVol = input.ReadFloat();
          break;
        }
        case 29: {
          EffectVol = input.ReadFloat();
          break;
        }
        case 37: {
          VoiceVol = input.ReadFloat();
          break;
        }
        case 40: {
          MasterVolOn = input.ReadBool();
          break;
        }
        case 48: {
          BgmVolOn = input.ReadBool();
          break;
        }
        case 56: {
          EffectVolOn = input.ReadBool();
          break;
        }
        case 64: {
          VoiceVolOn = input.ReadBool();
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
