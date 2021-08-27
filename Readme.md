<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128547183/18.1.10%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T164287)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [CustomDataSource.cs](./CS/WebApplication1/CustomDataSource.cs) (VB: [CustomDataSource.vb](./VB/WebApplication1/CustomDataSource.vb))
* [CustomObjects.cs](./CS/WebApplication1/CustomObjects.cs) (VB: [CustomObjects.vb](./VB/WebApplication1/CustomObjects.vb))
* [Default.aspx](./CS/WebApplication1/Default.aspx) (VB: [Default.aspx](./VB/WebApplication1/Default.aspx))
* [Default.aspx.cs](./CS/WebApplication1/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebApplication1/Default.aspx.vb))
<!-- default file list end -->
# How to implement the copy / paste appointment operation via the context menu
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/t164287/)**
<!-- run online end -->


<p>To implement the copy / paste appointment operation via an appointment context menu, the following approach is used in this sample:<br />1. Two custom items ("Copy" and "Paste") are added into the "view" and "appointment" context menus. This is implemented in the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_PopupMenuShowingtopic">ASPxScheduler.PopupMenuShowing</a> event handler.<br /><br />2. The availability of these items is managed in the client-side Popup event handler of the mentioned menus (the "Paste" item must be disabled if there is no copied appointment). Please refer to the javascript <strong>OnClientPopupMenuShowing</strong> method.<br /><br />3. When the "Copy" item is clicked, the ID of a currently selected appointment is stored as a HiddedField value. When the "Paste" item is clicked, a custom callback operation is executed, which results in copying the selected appointment on the server side.<br />

<br/>


