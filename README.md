**團隊成員**<br>
成員1:Ian林 Ian Lin<br>
成員2:Rupert趙<br>
成員3:Bob鄭<br>
成員4:Jamie涂<br>
成員5:Risa林<br>


# BPMPlus

<h1 align="center" style="font-weight: 700">Pawsmore｜BPMPlus 公司內部系統 </h1>
<div align="center" style="margin-bottom:24px">

### ｜[網站連結]([https://rocket-pawsmore.vercel.app/](https://bpmplus.azurewebsites.net/Login/Index?ReturnUrl=%2F))｜[設計稿](https://www.figma.com/board/X8XUObtwqiBiYyRZzByqDH/BPM-Plus?t=f1AYiT7rp1yfyWhW-0)
</div>
<br/>

## 設計沿革

    此服務旨對企業內部資源分配及跨部門協作提供一個有效率、權限劃分清楚的平台，並提供企業內部良好的客製化、彈性設計。包括工單處理流程、人員管理、工單流程追蹤、專案管理以及會議室預約系統

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

## 建議體驗流程

- 可參考腳本執行 https://docs.google.com/spreadsheets/d/106yY2lQMB4msqVr9MGo6ASZf00H1srSecuhV5qU9HpQ/edit?gid=186436850#gid=186436850

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

## Git 協作規範

### Branch

{成員姓名}_ {功能}  

ex.  Risa_Login

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
