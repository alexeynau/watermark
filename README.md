# OverlayApp
[![.github/workflows/build.yml](https://github.com/alexeynau/watermark/actions/workflows/build.yml/badge.svg)](https://github.com/alexeynau/watermark/actions/workflows/build.yml) 

OverlayApp — это приложение на WPF и Windows Forms, которое предоставляет функциональность наложения текста на экран с возможностью изменения цвета, текста и прозрачности через трей-меню.

## Функциональность

- **Трей-меню**:
  - Показать/Спрятать наложение.
  - Изменить цвет текста.
  - Изменить текст наложения.
  - Изменить прозрачность текста.
  - Выход из приложения.

- **Наложение текста**:
  - Текст отображается поверх всех окон.
  - Настраиваемый цвет, текст и прозрачность.

- **Управление видимостью**
  - Через меню трея
  - Дважды нажать на иконку трея
  - Сочетание клавиш Ctrl+Shift+H 

## Установка

1. Убедитесь, что у вас установлен .NET SDK версии 9.0 или выше.
2. Склонируйте репозиторий:
```bash
git clone <URL репозитория>
cd watermark
```
3. Постройте проект:
```bash
dotnet build  .\OverlayApp.csproj
dotnet publish .\OverlayApp.csproj -c Release -r win-x64 --self-contained true
```
## Запуск
Для запуска приложения выполните:
```bash
dotnet run .\OverlayApp.csproj
```

## Требования
* Windows 10 или выше.
* .NET 9.0 или выше.