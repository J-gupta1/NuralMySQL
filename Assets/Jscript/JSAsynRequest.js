
function changeCSSDropDown() {
    try {
        $("select").focusin(function() {
            $(this).removeClass('formselect');
            $(this).addClass('expand');
        });
        $("select").focus(function() {
            $(this).removeClass('formselect');
            $(this).addClass('expand');
        });
        $("select").blur(function() {

            $(this).removeClass('expand');
            $(this).addClass('formselect');
        });
        $("select").focusout(function() {

            $(this).removeClass('expand');
            $(this).addClass('formselect');
        });

        $("select").change(function() {

            $(this).removeClass('expand');
            $(this).addClass('formselect');
        });

        $("select").change(function() {

            $(this.blur()).removeClass('expand');
            $(this.blur()).addClass('formselect');
        });
    } catch (e) {

    }

}

  

var _updateProgressDiv;
function pageLoad(sender, args) {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

    _updateProgressDiv = $get('updateProgressDiv');
}

function beginRequest(sender, args) {
    var updateProgressDivBounds = Sys.UI.DomElement.getBounds(_updateProgressDiv);
    //var x = Math.round((screen.width / 2)) - Math.round(updateProgressDivBounds.width / 2);
    //var y = Math.round((screen.height / 2)) - Math.round(updateProgressDivBounds.height / 2);
    _updateProgressDiv.style.display = '';
    _updateProgressDiv.style.zindex = 999;
    //                 Sys.UI.DomElement.setLocation(_updateProgressDiv, 436, 366); 
  //  Sys.UI.DomElement.setLocation(_updateProgressDiv, x, y);
}
function endRequest(sender, args) {
    _updateProgressDiv.style.display = 'none';
   changeCSSDropDown();
  
}
     
function CloseDragger() {
    document.getElementById('pnlDragger').style.display = "none";
    document.getElementById('pnlDragger').style.visibility = "hidden";
}

/* Start: Char Left in a TextBox script */
function textCounter(d, s, r, hdn) {
    var field = document.getElementById(d + "_" + s);
    var cntfield = document.getElementById(d + "_" + r);
    var cntfieldHdn = document.getElementById(d + "_" + hdn);
    var maxlimit = field.getAttribute("CharsLength");
    if (field.value.length > parseInt(maxlimit)) {
        field.value = field.value.substring(0, parseInt(maxlimit));
    } else {
        cntfieldHdn.value = parseInt(maxlimit) - field.value.length;
    }
    cntfield.innerText = cntfieldHdn.value;
}

function SetValueOnPostback(d, r, hdn) {
    var cntfield = document.getElementById(d + "_" + r);
    var cntfieldHdn = document.getElementById(d + "_" + hdn);
    cntfield.innerText = cntfieldHdn.value;

}
/* End: Char Left in a TextBox script */ 
