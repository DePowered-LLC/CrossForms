#if IS_WIN_X64 || IS_WIN_X86 || IS_WIN_ARM
global using CrossForms.Native.Win32;

#elif IS_LINUX_X64 || IS_LINUX_ARM
global using CrossForms.Native.Stub;

#elif IS_OSX_X64 || IS_OSX_ARM64
global using CrossForms.Native.MacOS;

#else
global using CrossForms.Native.Stub;
#endif
