

/**
* Grid View Transfer Functions
* These are handy functions I use all the time.
*
* By Seth Banks (webmaster at subimage dot com)
* http://www.subimage.com/
*
* Up to date code can be found at http://www.subimage.com/dhtml/
*
* This code is free for you to use anywhere, just keep this comment block.
*/

/**
* X-browser event handler attachment and detachment
* TH: Switched first true to false per http://www.onlinetools.org/articles/unobtrusivejavascript/chapter4.html
*
* @argument obj - the object to attach event to
* @argument evType - name of the event - DONT ADD "on", pass only "mouseover", etc
* @argument fn - function to call
*/

function findControlIDTable(ServerID) {
    var FinalID;
    var tblcol = document.getElementsByTagName("Table");
    var lenght1 = tblcol.length;
    for (var i = 0; i < lenght1; i++) {
        if (tblcol[i].id != "") {
            var gridid = String(tblcol[i].id);
            var str2 = gridid.split("_");
            var strlength = str2.length;
            if (str2[strlength - 1] == String(ServerID)) {
                FinalID = gridid;
            }
        }
    }
    return FinalID;
}

function findControlIDBody(ServerID) {
    var FinalID;
    var tblcol = document.getElementsByTagName("tbody");
    var lenght1 = tblcol.length;
    for (var i = 0; i < lenght1; i++) {
        if (tblcol[i].id != "") {
            var gridid = String(tblcol[i].id);
            var str2 = gridid.split("_");
            var strlength = str2.length;
            if (str2[strlength - 1] == String(ServerID)) {
                FinalID = gridid;
            }
        }
    }
    return FinalID;
}


function findControlIDLabel(ServerID) {
   
    var FinalID;
    var tblcol = document.getElementsByTagName("SPAN");
    var lenght1 = tblcol.length;
    for (var i = 0; i < lenght1; i++) {
        if (tblcol[i].id != "") {
            var gridid = String(tblcol[i].id);
            var str2 = gridid.split("_");
            var strlength = str2.length;
            if (str2[strlength - 1] == String(ServerID)) {
                FinalID = gridid;
            }
        }
    }
    return FinalID;
}



function findcontrolIDInput(ServerID) {
    var FinalID;
    var tblcol = document.getElementsByTagName("Input");
    var lenght1 = tblcol.length;
    for (var i = 0; i < lenght1; i++) {
        if (tblcol[i].id != "") {
            var gridid = String(tblcol[i].id);
            var str2 = gridid.split("_");
            var strlength = str2.length;
            if (str2[strlength - 1] == String(ServerID)) {
                FinalID = gridid;
            }
        }
    }
    return FinalID;
}


function hithere(gridId1, lblid) {
     
    var FinalID = findControlIDTable(gridId1);
    var grid = document.getElementById(FinalID);
    var rowcount = grid.getElementsByTagName("TR").length;
    var totcolumns = grid.getElementsByTagName("TD").length;
    var columncount = totcolumns / (rowcount - 1);
    var div1 = document.getElementById("div1");
    var oTable = document.createElement("table");
    div1.appendChild(oTable);
    var oTHead = document.createElement("thead");
    var oBody = document.createElement("tbody");
    oTable.appendChild(oBody);
    oTable.appendChild(oTHead);
    var oRow = document.createElement("tr");
    oTHead.appendChild(oRow);
    for (var i = 0; i < columncount + 2; i++) {
        var oCell = document.createElement("th");
        if (i < columncount - 1) {
            oCell.innerHTML = grid.rows[0].cells[i].innerHTML;
            oCell.setAttribute("bgColor", "red");
        }
        if (i == columncount - 1) {
            oCell.innerHTML = String("Quantity");
            oCell.setAttribute("bgColor", "red");
        }
        if (i == columncount) {
            oCell.innerHTML = String("Quantity");
            oCell.setAttribute("bgColor", "red");
        }
        if (i == columncount + 1) {
            oCell.style.visibility = 'hidden';
        }

        oRow.appendChild(oCell);
    }
    var a = 1;
    var b = 1;
    var oRow1;
    var ocell;

    debugger;
    for (var i = 1; i < rowcount - 1; i++) {
            if (grid.rows[i].cells[columncount - 1].getElementsByTagName('Input')[0].checked == true) {
            oRow1 = document.createElement("tr");
            oBody.appendChild(oRow1);
            b = 1;
            for (var j = 0; j <= columncount + 1; j++) {
                ocell = (document.createElement("td"));
                if (j < columncount - 1) {
                    ocell.innerHTML = grid.rows[i].cells[j].innerHTML;

                }
                if (j == columncount - 1 || j == columncount) {
                    var txt = (document.createElement("input"));
                    txt.id = String('TextBox' + a + b);
                    txt.name = String('TextBox' + a + b);

                    if (b == 1) {
                        var str1 = String(txt.id);
                        var str2 = String(document.getElementById(grid.rows[i].cells[2].getElementsByTagName('SPAN')[0].id).firstChild.data);
                        var str3 = String(document.getElementById(grid.rows[i].cells[4].getElementsByTagName('SPAN')[0].id).firstChild.data);
                        var str4 = String(String(String(a).concat(String(b + 1))));
                        var finalstr = String("changeme(" + str1 + "," + str2 + "," + str3 + "," + str4 + ")");
                        txt.setAttribute("onChange", finalstr);
                    }

                    ocell.appendChild(txt)
                    b++;
                }
                if (j == columncount + 1) {
                   
                    var txt = (document.createElement("input"));
                    txt.id = String('TextBox' + a + b);
                    txt.name = String('TextBox' + a + b);
                    txt.value = String(document.getElementById(grid.rows[i].cells[columncount - 2].getElementsByTagName('SPAN')[0].id).firstChild.data);
                    txt.style.visibility = 'hidden';
                    ocell.appendChild(txt);
                    b++;
                }

                oRow1.appendChild(ocell);
            }
            a++;
        }
    }
    debugger;
   
    
//    var btnid1 = findcontrolIDInput(btnID);
//    var btn = document.getElementById(btnid1);
//    btn.disabled = true;
    return false;

}

function hithereforOutwards(gridId1, btnID) {

    var FinalID = findControlIDTable(gridId1);
    var grid = document.getElementById(FinalID);
    var rowcount = grid.getElementsByTagName("TR").length;
    var totcolumns = grid.getElementsByTagName("TD").length;
    var columncount = totcolumns / (rowcount - 1);
    var div1 = document.getElementById("div1");
    if (String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) == "0") {
        var oTable = document.createElement("table");
        oTable.name = "tblDyn";
        oTable.id = "tblDyn";
        div1.appendChild(oTable);
        var oTHead = document.createElement("thead");
        var oBody = document.createElement("tbody");
        oBody.name = "bdyDyn";
        oBody.id = "bdyDyn";
        oTable.appendChild(oBody);
        oTable.appendChild(oTHead);
        var oRow = document.createElement("tr");
        oTHead.appendChild(oRow);

        for (var i = 0; i < 6; i++) {
            var oCell = document.createElement("th");
            if (i < columncount - 1) {
                oCell.innerHTML = grid.rows[0].cells[i].innerHTML;
                oCell.setAttribute("bgColor", "skyblue");
            }
            if (i == 2) {
                oCell.innerHTML = String("Quantity");
                oCell.setAttribute("bgColor", "skyblue");
            }
            if (i == 3) {
                oCell.innerHTML = String("BatchNumber");
                oCell.setAttribute("bgColor", "skyblue");
            }
            if (i == 4) {
                oCell.innerHTML = String("SerialNumber");
                oCell.setAttribute("bgColor", "skyblue");
            }

            if (i == 5) {
                oCell.style.visibility = 'hidden';
            }

            oRow.appendChild(oCell);
        }

        document.getElementById(findControlIDLabel(btnID)).firstChild.data = 1;
    }
    else {
        oBody = document.getElementById(findControlIDBody("bdyDyn"))
     
    }
    
//    var a = 1;
    var b = 1;
    var oRow1;
    var ocell;


    for (var i = 1; i < rowcount ; i++) {
       
        if (grid.rows[i].cells[columncount - 1].getElementsByTagName('Input')[0].checked == true) {
            oRow1 = document.createElement("tr");
            oBody.appendChild(oRow1);
            b = 1;
            for (var j = 0; j <= columncount + 1; j++) {
                ocell = (document.createElement("td"));
                if (j < 2) {
                    ocell.innerHTML = grid.rows[i].cells[j].innerHTML;

                }
                if (j == 2) {
                    var txt = (document.createElement("input"));
                    txt.id = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    txt.name = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    ocell.appendChild(txt)
                    b++;
                }
                if (j == 3) {
                    var txt = (document.createElement("input"));
                    txt.id = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    txt.name = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    if (String(document.getElementById(grid.rows[i].cells[2].getElementsByTagName('SPAN')[0].id).firstChild.data) != "2") {
                        txt.disabled = true;
                    }
                    ocell.appendChild(txt)
                    b++;

                }
                if (j == 4) {
                    var txt = (document.createElement("input"));
                    txt.id = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    txt.name = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    if (String(document.getElementById(grid.rows[i].cells[2].getElementsByTagName('SPAN')[0].id).firstChild.data) != "3") {
                        txt.disabled = true;
                    }
                    ocell.appendChild(txt)
                    b++;

                }


                if (j == 5) {

                    var txt = (document.createElement("input"));
                    txt.id = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    txt.name = String('TextBox' + String(document.getElementById(findControlIDLabel(btnID)).firstChild.data) + b);
                    txt.value = String(document.getElementById(grid.rows[i].cells[3].getElementsByTagName('SPAN')[0].id).firstChild.data);
                    txt.style.visibility = 'hidden';
                    ocell.appendChild(txt);
                    b++;
                }

                oRow1.appendChild(ocell);
            }
            (document.getElementById(findControlIDLabel(btnID)).firstChild.data)++;
        }
    }
  
   
//    var btnid1 = findcontrolIDInput(btnID);
//    var btn = document.getElementById(btnid1);
//    btn.disabled = true;
    return false;
}

function changeme(txtobj, inhandqty, cost, id) {
    var txt = document.getElementsByName(txtobj.name).item(0);
    if (txt.value == 0) {

        alert("Please enter some quantity");
        return false;
    }
    if (isNaN(txt.value)) {
        alert("Please enter valid  numerical value");
        return false;

    }
    if (txt.value > inhandqty) {
        alert("The entered quantity must be less than in hand quantity");
        txt.value = String("");
        return false;
    }

    var quantity = txt.value;
    var amount = quantity * cost;
    var txt2 = document.getElementsByName(String.format("TextBox{0}", id)).item(0);
    txt2.value = amount;
    return false;
}


