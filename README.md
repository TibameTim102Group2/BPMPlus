<h1 align="center" style="font-weight: 700">BPMPlus | 企業流程管理系統 </h1>

<br/>

![image](https://github.com/user-attachments/assets/a131ff5d-432c-4041-b111-b9da26cef513)


<div align="center" style="margin-bottom:24px">

### [設計稿](https://www.figma.com/board/X8XUObtwqiBiYyRZzByqDH/BPM-Plus?t=f1AYiT7rp1yfyWhW-0)
</div>

<br>

## 設計沿革

此服務旨對企業內部流程管理及跨部門協作提供一個有效率、權限劃分清楚的平台。功能包括工單處理流程、人員管理、工單流程追蹤、專案管理以及會議室預約系統
<br>
## 功能清單

- 首頁
- 登入登出
- 修改密碼
- 忘記密碼
- 查詢工單
- 待辦清單
- 新增工單
- 工單細節
- 修改工單
- 審核工單
- 審核發信
- 查詢專案
- 新建專案
- 專案細節
- 修刪專案
- 新增會議室
- 會議室(查/刪/修)
- 後台-新增工單類別
- 後台-查刪工單
- 後台-人員新增
- 後台-人員查詢
- 後台-人員修改
<br>

## 建議體驗流程

- 可參考腳本執行 https://docs.google.com/spreadsheets/d/1YRZnviZospUyJJe2puAvFB74tEEhu_Xz95ZeoM4_t38/edit?gid=161174274#gid=161174274
<br>

## 程式執行環境

- Visual Studio 2022
  <br> 下載 Visual studio 2022 社群版本，開啟 BPMPlus.csproj，然後執行，並執行 update-database -context ApplicationDbContext
  
- SQL server management studio

## 資料遷移指令(套件管理主控台)

```
update-database -context ApplicationDbContext //更新 DB 至最新的 migration script
```

```
update-database {migration name} -context ApplicationDbContext //更新 DB 至指定版本
```

```
add-migration "YourMigrationName" -context ApplicationDbContext // 依照 model file 以及 applicationDbContext 的修改自動產生資料遷移腳本
```

```
remove-migration -context ApplicationDbContext // 移除最新的 migration
``` 

## appsetting.json

```
"ConnectionStrings": {
  "DefaultConnection": "Server={YourServer};Database=BPMPlus;Encrypt=True;Trusted_Connection=True;MultipleActiveResultSets=true"
},
```

## 專案資料架構

```
BPMPlus
  ├── Connected Services // 連線資料庫的資訊
  ├── Properties // 啟動服務的 configure 
  ├── wwwroot // 用戶端程式庫以及引用檔案 (圖檔、CSS、Javascript)
  ├── Attributes // 掛載於controller 的 action filter
  ├── Controllers // 各個頁面、DB table 的控制器
  ├── Data // 資料遷移腳本，以及 EF model 對應實體關係
  ├── Models // 對應資料庫各資料表的類別宣告
  ├── Services // 串接外部服務以及可抽取共用之程式邏輯
  ├── ViewModels // 後端接收前端送來的資料，以及發送至前端的資料，所用的類別 Schema 宣告
  ├── Views // 呈現於網頁的視圖原型 Razor 文件，以及執行時期的 JS 函數
  ├── .gitignore // 脫離版控的檔案清單
  ├── Program.cs // 主程式
  ├── appsetting.json // 環境變數
  └── libman.json // 用戶端程式庫套件管理
```
<br>

## Git 協作規範

### Branch

{成員姓名}_ {功能}  

ex.  Risa_Login
<br>

## 技術規格

### 後端

- C#
- [ASP.NET](http://ASP.NET) Core MVC
- MSSQL
- Postman
- LINQ
- EF Core
- Azure app service

### 前端

- Vue
- jQuery
- JavaScript
- Razor
- CSS
- jQuery.ajax
- Fetch
- Axios
