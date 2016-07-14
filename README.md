# mvvmMenuSystem
a menusystem use uFrame MVVM

## Quick Start

1. Clone the essential parts in to local folder Asset/Plugins/:

	[uFrame/Core](https://github.com/uFrame/Core) |
	[uFrame/Architect](https://github.com/uFrame/Architect) |
	[uFrame/MVVM](https://github.com/uFrame/MVVM) |
	[UniRx](https://github.com/neuecc/UniRx/releases)

2. Download the latest release from the release page and Unzip

	[mvvmMenuSystem](https://github.com/fuutou89/mvvmMenuSystem/releases) 

3. Create a uFrame Database or Open your database

4. Select Import and choose "menuWorkspace.ufdata" to open

5. Open menuWorkspace from your Workspaces page

6. Cope "MiscScripts" and "Resources" into folder "MenuSystem"

7. Create a MVVMGraph or open an exist one

8. Create a new Scene Type node and rename to "MenuScene" and update Kernel

9. Create a new Scene "MenuScene"

10. In "MenuScene", Create an empty gameobjct call "MenuRootView" and attach "MenuRootView" Component

11. Attach "ShieldView" Component into prefab "uGUIShield"

12. Create Scene "MainMenu" and Create an empty gameobject call "MainMenu" and attach "MainMenuView" Component

13. Create Scene "Loading" and Create an empty gameobjct call "Loading" and attach "PanelView" Component

14. Add kernel scene and other 3 scenes you just create into build setting

15. Open "MenuScene" and Hit Play to see the magic !!
