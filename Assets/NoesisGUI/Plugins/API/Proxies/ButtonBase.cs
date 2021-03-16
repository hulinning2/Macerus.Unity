//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Input;

namespace Noesis
{

public class ButtonBase : ContentControl {
  internal new static ButtonBase CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new ButtonBase(cPtr, cMemoryOwn);
  }

  internal ButtonBase(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(ButtonBase obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  protected ButtonBase() {
  }

  #region Events

  #region Click
  public delegate void ClickHandler(object sender, RoutedEventArgs e);
  public event ClickHandler Click {
    add {
      if (!_Click.ContainsKey(swigCPtr.Handle)) {
        _Click.Add(swigCPtr.Handle, null);

        NoesisGUI_PINVOKE.BindEvent_ButtonBase_Click(_raiseClick, swigCPtr.Handle);
        if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      }

      _Click[swigCPtr.Handle] += value;
    }
    remove {
      if (_Click.ContainsKey(swigCPtr.Handle)) {

        _Click[swigCPtr.Handle] -= value;

        if (_Click[swigCPtr.Handle] == null) {
          NoesisGUI_PINVOKE.UnbindEvent_ButtonBase_Click(_raiseClick, swigCPtr.Handle);
          if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();

          _Click.Remove(swigCPtr.Handle);
        }
      }
    }
  }

  internal delegate void RaiseClickCallback(IntPtr cPtr, IntPtr sender, IntPtr e);
  private static RaiseClickCallback _raiseClick = RaiseClick;

  [MonoPInvokeCallback(typeof(RaiseClickCallback))]
  private static void RaiseClick(IntPtr cPtr, IntPtr sender, IntPtr e) {
    try {
      if (!_Click.ContainsKey(cPtr)) {
        throw new InvalidOperationException("Delegate not registered for Click event");
      }
      if (sender == IntPtr.Zero && e == IntPtr.Zero) {
        _Click.Remove(cPtr);
        return;
      }
      if (Noesis.Extend.Initialized) {
        ClickHandler handler = _Click[cPtr];
        if (handler != null) {
          handler(Noesis.Extend.GetProxy(sender, false), new RoutedEventArgs(e, false));
        }
      }
    }
    catch (Exception exception) {
      Noesis.Error.SetNativePendingError(exception);
    }
  }

  static Dictionary<IntPtr, ClickHandler> _Click =
      new Dictionary<IntPtr, ClickHandler>();
  #endregion

  #endregion

  public ICommand Command {
    get {
      return (ICommand)GetCommandHelper();
    }
    set {
      SetCommandHelper(value);
    }
  }

  public static DependencyProperty ClickModeProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_ClickModeProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty CommandProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_CommandProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty CommandParameterProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_CommandParameterProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty CommandTargetProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_CommandTargetProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public static DependencyProperty IsPressedProperty {
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_IsPressedProperty_get();
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (DependencyProperty)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public ClickMode ClickMode {
    set {
      NoesisGUI_PINVOKE.ButtonBase_ClickMode_set(swigCPtr, (int)value);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      ClickMode ret = (ClickMode)NoesisGUI_PINVOKE.ButtonBase_ClickMode_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public object CommandParameter {
    set {
      NoesisGUI_PINVOKE.ButtonBase_CommandParameter_set(swigCPtr, Noesis.Extend.GetInstanceHandle(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    }
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_CommandParameter_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public UIElement CommandTarget {
    set {
      NoesisGUI_PINVOKE.ButtonBase_CommandTarget_set(swigCPtr, UIElement.getCPtr(value));
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_CommandTarget_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return (UIElement)Noesis.Extend.GetProxy(cPtr, false);
    }
  }

  public bool IsPressed {
    get {
      bool ret = NoesisGUI_PINVOKE.ButtonBase_IsPressed_get(swigCPtr);
      if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  private object GetCommandHelper() {
    IntPtr cPtr = NoesisGUI_PINVOKE.ButtonBase_GetCommandHelper(swigCPtr);
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    return Noesis.Extend.GetProxy(cPtr, false);
  }

  private void SetCommandHelper(object command) {
    NoesisGUI_PINVOKE.ButtonBase_SetCommandHelper(swigCPtr, Noesis.Extend.GetInstanceHandle(command));
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.ButtonBase_GetStaticType();
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }


  internal new static IntPtr Extend(string typeName) {
    IntPtr nativeType = NoesisGUI_PINVOKE.Extend_ButtonBase(Marshal.StringToHGlobalAnsi(typeName));
    if (NoesisGUI_PINVOKE.SWIGPendingException.Pending) throw NoesisGUI_PINVOKE.SWIGPendingException.Retrieve();
    return nativeType;
  }
}

}
