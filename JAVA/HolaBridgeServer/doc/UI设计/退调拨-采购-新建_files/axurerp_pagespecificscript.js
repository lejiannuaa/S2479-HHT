﻿
var PageName = '退调拨-采购-新建';
var PageId = '9a60f84beba74469ae76c3a08a52a1cf'
var PageUrl = '退调拨-采购-新建.html'
document.title = '退调拨-采购-新建';
var PageNotes = 
{
"pageName":"退调拨-采购-新建",
"showNotesNames":"False"}
var $OnLoadVariable = '';

var $CSUM;

var hasQuery = false;
var query = window.location.hash.substring(1);
if (query.length > 0) hasQuery = true;
var vars = query.split("&");
for (var i = 0; i < vars.length; i++) {
    var pair = vars[i].split("=");
    if (pair[0].length > 0) eval("$" + pair[0] + " = decodeURIComponent(pair[1]);");
} 

if (hasQuery && $CSUM != 1) {
alert('Prototype Warning: The variable values were too long to pass to this page.\nIf you are using IE, using Firefox will support more data.');
}

function GetQuerystring() {
    return '#OnLoadVariable=' + encodeURIComponent($OnLoadVariable) + '&CSUM=1';
}

function PopulateVariables(value) {
    var d = new Date();
  value = value.replace(/\[\[OnLoadVariable\]\]/g, $OnLoadVariable);
  value = value.replace(/\[\[PageName\]\]/g, PageName);
  value = value.replace(/\[\[GenDay\]\]/g, '8');
  value = value.replace(/\[\[GenMonth\]\]/g, '3');
  value = value.replace(/\[\[GenMonthName\]\]/g, '三月');
  value = value.replace(/\[\[GenDayOfWeek\]\]/g, '星期四');
  value = value.replace(/\[\[GenYear\]\]/g, '2012');
  value = value.replace(/\[\[Day\]\]/g, d.getDate());
  value = value.replace(/\[\[Month\]\]/g, d.getMonth() + 1);
  value = value.replace(/\[\[MonthName\]\]/g, GetMonthString(d.getMonth()));
  value = value.replace(/\[\[DayOfWeek\]\]/g, GetDayString(d.getDay()));
  value = value.replace(/\[\[Year\]\]/g, d.getFullYear());
  return value;
}

function OnLoad(e) {

if (true) {

	SetPanelVisibility('u13','','fade',500);

	SetPanelVisibility('u41','','fade',500);

	SetPanelVisibility('u44','','fade',500);

}

}

var u31 = document.getElementById('u31');

var u36 = document.getElementById('u36');

var u16 = document.getElementById('u16');
gv_vAlignTable['u16'] = 'top';
var u17 = document.getElementById('u17');

var u28 = document.getElementById('u28');
gv_vAlignTable['u28'] = 'top';
var u29 = document.getElementById('u29');

var u8 = document.getElementById('u8');

var u30 = document.getElementById('u30');
gv_vAlignTable['u30'] = 'top';
var u21 = document.getElementById('u21');

var u6 = document.getElementById('u6');

var u58 = document.getElementById('u58');

u58.style.cursor = 'pointer';
if (bIE) u58.attachEvent("onclick", Clicku58);
else u58.addEventListener("click", Clicku58, true);
function Clicku58(e)
{
windowEvent = e;


if (true) {

	NewWindow("resources/Other.html#other=" + encodeURI("请求成功！"), "", "directories=0, height=300, location=0, menubar=0, resizable=1, scrollbars=1, status=0, toolbar=0, width=300", true, 300, 300);

}

}

var u32 = document.getElementById('u32');
gv_vAlignTable['u32'] = 'top';
var u59 = document.getElementById('u59');
gv_vAlignTable['u59'] = 'center';
var u60 = document.getElementById('u60');
gv_vAlignTable['u60'] = 'top';
var u15 = document.getElementById('u15');

var u13 = document.getElementById('u13');

var u14 = document.getElementById('u14');

var u4 = document.getElementById('u4');

u4.style.cursor = 'pointer';
if (bIE) u4.attachEvent("onclick", Clicku4);
else u4.addEventListener("click", Clicku4, true);
function Clicku4(e)
{
windowEvent = e;


if (true) {

	self.location.href="出货作业.html" + GetQuerystring();

}

}

var u38 = document.getElementById('u38');

var u43 = document.getElementById('u43');
gv_vAlignTable['u43'] = 'top';
var u44 = document.getElementById('u44');

var u40 = document.getElementById('u40');

var u1 = document.getElementById('u1');
gv_vAlignTable['u1'] = 'center';
var u37 = document.getElementById('u37');
gv_vAlignTable['u37'] = 'top';
var u26 = document.getElementById('u26');
gv_vAlignTable['u26'] = 'top';
var u41 = document.getElementById('u41');

var u10 = document.getElementById('u10');
gv_vAlignTable['u10'] = 'top';
var u11 = document.getElementById('u11');

var u3 = document.getElementById('u3');
gv_vAlignTable['u3'] = 'center';
var u27 = document.getElementById('u27');

var u39 = document.getElementById('u39');
gv_vAlignTable['u39'] = 'top';
var u9 = document.getElementById('u9');
gv_vAlignTable['u9'] = 'center';
var u35 = document.getElementById('u35');
gv_vAlignTable['u35'] = 'top';
var u57 = document.getElementById('u57');

var u7 = document.getElementById('u7');
gv_vAlignTable['u7'] = 'center';
var u42 = document.getElementById('u42');

var u12 = document.getElementById('u12');
gv_vAlignTable['u12'] = 'center';
var u23 = document.getElementById('u23');

var u24 = document.getElementById('u24');
gv_vAlignTable['u24'] = 'top';
var u25 = document.getElementById('u25');

var u46 = document.getElementById('u46');
gv_vAlignTable['u46'] = 'top';
var u53 = document.getElementById('u53');
gv_vAlignTable['u53'] = 'top';
var u56 = document.getElementById('u56');
gv_vAlignTable['u56'] = 'center';
var u54 = document.getElementById('u54');

var u2 = document.getElementById('u2');

var u18 = document.getElementById('u18');
gv_vAlignTable['u18'] = 'top';
var u19 = document.getElementById('u19');

var u55 = document.getElementById('u55');

var u20 = document.getElementById('u20');
gv_vAlignTable['u20'] = 'top';
var u5 = document.getElementById('u5');
gv_vAlignTable['u5'] = 'center';
var u48 = document.getElementById('u48');
gv_vAlignTable['u48'] = 'center';
var u22 = document.getElementById('u22');
gv_vAlignTable['u22'] = 'top';
var u49 = document.getElementById('u49');

var u47 = document.getElementById('u47');

u47.style.cursor = 'pointer';
if (bIE) u47.attachEvent("onclick", Clicku47);
else u47.addEventListener("click", Clicku47, true);
function Clicku47(e)
{
windowEvent = e;


if (true) {

	NewWindow("resources/Other.html#other=" + encodeURI("请求成功！"), "", "directories=0, height=300, location=0, menubar=0, resizable=1, scrollbars=1, status=0, toolbar=0, width=300", true, 300, 300);

}

}

var u50 = document.getElementById('u50');
gv_vAlignTable['u50'] = 'center';
var u51 = document.getElementById('u51');
gv_vAlignTable['u51'] = 'top';
var u45 = document.getElementById('u45');

var u52 = document.getElementById('u52');

var u33 = document.getElementById('u33');

var u34 = document.getElementById('u34');
gv_vAlignTable['u34'] = 'top';
var u0 = document.getElementById('u0');

if (window.OnLoad) OnLoad();
