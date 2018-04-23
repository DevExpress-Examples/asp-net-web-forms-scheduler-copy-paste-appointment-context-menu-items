# How to implement the copy / paste appointment operation via the context menu


<p>To implement the copy / paste appointment operation via an appointment context menu, the following approach is used in this sample:<br />1. Two custom items ("Copy" and "Paste") are added into the "view" and "appointment" context menus. This is implemented in the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_PopupMenuShowingtopic">ASPxScheduler.PopupMenuShowing</a> event handler.<br /><br />2. The availability of these items is managed in the client-side Popup event handler of the mentioned menus (the "Paste" item must be disabled if there is no copied appointment). Please refer to the javascript <strong>OnClientPopupMenuShowing</strong> method.<br /><br />3. When the "Copy" item is clicked, the ID of a currently selected appointment is stored as a HiddedField value. When the "Paste" item is clicked, a custom callback operation is executed, which results in copying the selected appointment on the server side.<br />Since this functionality is implemented in the client-side Menu.ItemClick event handler, we need to also invoke standard ASPxScheduler actions for predefined items: <br />"<strong>aspxSchedulerOnAptMenuClick</strong>" - for predefined items of the "appointment" context menu<br />"<strong>aspxSchedulerOnViewMenuClick</strong>" - for predefined items of the "view" context menu </p>

<br/>


