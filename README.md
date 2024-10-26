# OneProject

> 所有功能通过Desktop来展现

## OneProject Desktop

> 仅支持 Windows

### 功能

- Windows
  - [ ] 注册表操作：查询、备份、删除、加载
  - [ ] 系统还原点：查询、备份、删除、还原提醒（只是个提醒）
- 下载器
- 基础功能
  - [x] 单例应用程序：仅启动一个应用程序
  - [x] 利用Github Action自动推送Release
  - [ ] 检查更新：启动时，连接github，检查最新版本，给到下载链接

## 引用第三方库

- [FastEnum.Extensions.Generator](https://github.com/D4nyi/FastEnum.Extensions.Generator)
  - MembersCount (field)
  - GetValues
  - GetUnderlyingValues
  - GetNames
  - HasFlag
  - IsDefined
  - FastToString
  - FastToString with format option
  - GetDescription
  - TryParse (string/System.ReadOnlySpan)
  - TryParseIgnoreCase (string/System.ReadOnlySpan)


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
