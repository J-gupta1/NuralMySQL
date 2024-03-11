function popupclose() {
    if (window.event.keyCode == 27)
        window.close();
}

function IsNumeric(Ctrl) {
    var ValidChars = "-0123456789";
    var IsNumber = true;
    var Char;
    if (Ctrl.value != "") {
        for (i = 0; i < Ctrl.value.length && IsNumber == true; i++) {
            Char = Ctrl.value.charAt(i);
            if (ValidChars.indexOf(Char) == -1) {
                IsNumber = false;
                alert("Please enter valid sales figure!");
                eval("document.Form1." + passName).focus();
                return IsNumber;
                break;
            }
        }

        return IsNumber;

    }
}

function ConfirmDelete() {
    return confirm("Are you sure to delete!");

}
function fncChkSize(source, args) {
    var Str = args.Value;
    if (Str.length < 251 && Str.length > 0)
    { args.IsValid = true; }
    else
    { args.IsValid = false; }
}
function fncChkSizeNarration(source, args) {
    var Str = args.Value;
    if (Str.length < 501 && Str.length > 0)
    { args.IsValid = true; }
    else
    { args.IsValid = false; }
}
