/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2012 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By    : Amit Agarwal
* Module        : User Controls
* Description   : DevExpress controls validations are not compatible with standard .net validations so
                  this file need to be add a refrence on UserControl if any page containing both devexpress
                  controls with validation setting  and standard .net framework vaidation then submitting button
                  need to call a method (i.e OnLinkButtonClick()) explicitly on the Event named (onClientClick) 
                  for validating the page. 
                  Exp: OnClientClick="return OnLinkButtonClick();"
                  Points to Remember
                  1. Add this property on submitting Button: OnClientClick="return OnLinkButtonClick();"
                  2. add this Line of Code to UserControl on PageLoad Event if UserControl has any of devExpress Control with 
                     Validation Setting:   ScriptManager.GetCurrent(this.Page).Scripts.Add(new ScriptReference("~/Assets/Scripts/DevExValidationCampatibility.js"));
                  3. Never Set any of Page Validator's ValidationGroup property on page otherwise validations will not fire.   
* ====================================================================================================
* Reviewed By :
====================================================================================================
Modification On   Change Code           Modified By          Modification    
---------------   -----------           -----------          ---------------------------------------  
====================================================================================================
*/

function OnASPxButtonClick(s, e) {
    var validationGroup = '';
    e.processOnServer = RaisePageValidation(validationGroup);
}
// Validation via standard LinkButton
function OnLinkButtonClick() {
    var validationGroup = '';
    return RaisePageValidation(validationGroup);
}

function RaisePageValidation(validationGroup) {
    var validationProcs = [RaiseDxValidation, RaiseStandardValidation];
    var result = true;
    try {
        for (var index = 0; index < validationProcs.length; index++)
            if (result != null & validationGroup != null) {
            result = validationProcs[index](validationGroup) && result;
        }
    }
    catch (e) {
        alert(e);
    }
    return result;
}

/* Different validation procs */
function RaiseDxValidation(validationGroup) {
    if (typeof (ASPxClientEdit) != "undefined" && typeof (ASPxClientEdit.ValidateGroup) == "function")
        return ASPxClientEdit.ValidateGroup(validationGroup);
    else
        return true;
}
function RaiseStandardValidation(validationGroup) {
    try {
        if (typeof (Page_IsValid) != "undefined" && Page_IsValid != null && typeof (Page_ClientValidate) == "function") {
            Page_ClientValidate(validationGroup);
            return Page_IsValid;
        }
    }
    catch (e) {
        alert(e);
    }
    return true;
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
    if (cntfield != null && cntfieldHdn != null) {
        cntfield.innerText = cntfieldHdn.value;
    }
}
/* End: Char Left in a TextBox script */ 

