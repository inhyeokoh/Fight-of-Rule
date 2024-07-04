# -*- mode: python ; coding: utf-8 -*-


a = Analysis(
    ['Core\\main.py'],
    pathex=['.\\..\\..\\..\\Users\\너굴이\\AppData\\Roaming\\Python\\Python39\\site-packages', '.\\..\\..\\..\\ProgramData\\Anaconda3\\envs\\main\\Lib\\site-packages', '.\\..\\..\\..\\ProgramData\\Anaconda3\\Lib\\site-packages', '.\\..\\..\\..\\ProgramData\\Anaconda3\\Lib'],
    binaries=[],
    datas=[],
    hiddenimports=[],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.datas,
    [],
    name='main',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=True,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
)
