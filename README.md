# OneProject

> 所有功能通过Desktop来展现

## OneProject Desktop

> 仅支持 Windows

### 功能

- Windows
  - [ ] 注册表操作
- 基础功能
  - [x] 单例应用程序：仅启动一个应用程序
  - [ ] 利用Github Action自动推送Release
  - [ ] 自动更新：启动时，连接github，自动更新

## 其他

- red tag info within Github action

```yml
# - name: Get Version From Tag
#   uses: olegtarasov/get-tag@v2.1.3
#   id: Tag
#   with:
#     tagRegex: "Release_(?<Package>.*)"
# - name: Show Tag
#   run: |
#     echo "${{steps.Tag.outputs.Package}}"
```
