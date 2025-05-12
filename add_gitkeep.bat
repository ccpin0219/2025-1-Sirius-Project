@echo off
chcp 65001 > nul

echo 🔍 빈 폴더 검색 중...

for /d /r %%d in (*) do (
    if exist "%%d" (
        dir /b "%%d" | findstr . >nul || (
            echo 📁 빈 폴더: %%d
            type nul > "%%d\.gitkeep"
        )
    )
)

echo.
echo ✅ 모든 빈 폴더에 .gitkeep 추가 완료!

echo.
echo [Enter 키를 누르면 종료됩니다...]
pause > nul
