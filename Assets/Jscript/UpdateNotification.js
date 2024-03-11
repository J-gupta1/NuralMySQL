////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                              MODAL AND SPINNY FUNCTIONS
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// the id of the error label
var ERROR_LABEL = "lblError";

// attach javascript
$(document).ready(function() {
    // grab the script manager
    var manager = Sys.WebForms.PageRequestManager.getInstance();
    
    // hook in our callbacks for the ASP.NET Ajax Postbacks
    manager.add_beginRequest(modalBeginRequest);
    manager.add_endRequest(modalEndRequest);

    // override the 5px grey border
    $.blockUI.defaults.css['border'] = '1px';
    $.blockUI.defaults.css['height'] = '50px';
    $.blockUI.defaults.css['width'] = '301px';
    //$.blockUI.defaults.css['background'] = 'transparent url(Images/background_image_here.png) no-repeat';    
});

function modalBeginRequest(sender, args) {
    // kill the scrollbars
    try
    {
        $('html').css({ "overflow": "hidden" });

        // show a modal popup
        $.blockUI({ message: $('#modalPopup') });

        // clear the error text
        $('#' + ERROR_LABEL).html('');
    }
    catch(err)
    {
       $.unblockUI();
//        $('html').css({ "overflow": "scroll" });

//    // detect if an error in postback occured.
//        if (args != null) 
//        {
//            var error = args.get_error();
//            if (error != null) {
//                $('#' + ERROR_LABEL).html(error.message);
//            }
//        }
    }
}

function modalEndRequest(sender, args) {
    // remove the modal popup
    $.unblockUI();

    // restore the scrollbars
    $('html').css({ "overflow": "scroll" });

    // detect if an error in postback occured.
    if (args != null) {
        var error = args.get_error();
        if (error != null) {
            $('#' + ERROR_LABEL).html(error.message);
        }
    }
}

