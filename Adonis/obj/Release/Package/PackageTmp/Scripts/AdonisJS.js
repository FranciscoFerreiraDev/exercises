var lista;
var DescriptionList;
var name = [];
var k = "";

function SearchCandidatesByROLE(role)
{
    ViewBag.Candidates;
}

function FillEditFormSkill(IdSkill, Name, IdCategory, Active)
{
    $("#OldId").val(IdSkill);
    $("#oldSkill").val(Name);
    $("#CategoryEdit").val(IdCategory);

    var cbActivated = $("#Activated");
    if(Active === 1)
        $("#Activated").val(1);
    else
        $("#Activated").val(0);
}

function FillEditFormRoleCandidacyAvailabilityLocation(Id, Name, Active)
{
    $("#OldId").val(Id);
    $("#newName").val(Name);
    
    var cbActivated = $("#Activated");
    if (Active === 1)
        $("#Activated").val(1);
    else
        $("#Activated").val(0);
}

function FillEditFormCandidate(IdCandidate, IdCRnS, IdRnS, Name, IdNationality,IdRole, IdSkill, Start, Finish, IdGrade, IdLocation, Availability, Gross, NET,
    Daily,RemunerationNotes, Status, Interview, Candidacy, UserEval, CandidateCode, MainExp, Activated, Description, BirthDate)
{
    ShowHideEditFormsCandidates(2);
    $('#id').val(IdCandidate);
    $('#crns').val(IdCRnS);
    $('#RnS').val(IdRnS);
    $('#UpdateName').val(Name);
    $('#EditNationality').val(IdNationality);
    $('#UpdateRole').val(IdRole);
    $('#UpdateSkill').val(IdSkill);
    $('#UpdateInit').val(Start);
    $('#UpdateEnd').val(Finish);
    $('#UpdateGrade').val(IdGrade);
    $('#UpdateCurrentPlace').val(IdLocation);
    $('#UpdateAvaiability').val(Availability);
    $('#UpdateGross').val(Gross);
    $('#UpdateNET').val(NET);
    $('#UpdateDailyGains').val(Daily);
    $('#UpdateRemunerationNotes').val(RemunerationNotes);
    $('#UpdateStatus').val(Status);
    $('#UpdateInterview').val(Interview);
    $('#UpdateCand').val(Candidacy);
    $('#UpdateEvaluation').val(UserEval);
    $('#UpdateCandidateCode').val(CandidateCode);
    $('#UpdateMainxp').val(MainExp);
    $('#UpdateActivated').val(Activated);
    $('#UpdateDesc').val(Description);

    var teste = BirthDate.split("-");
    var date = teste[0] + "-" + teste[1] + "-" + teste[2];
    $('#EditBirthDate').val(date);
}

function FillEditFormExp(IdRNS, IdCRNS, IdCandidate, IdRole, IdSkill, StartDate, FinishDate, IdGrade, Description, MainExp, CandidateName) {
    ShowHideEditFormsCandidates(5);
    $('#IdRnSERNS').val(IdRNS);
    $('#IdECRnS').val(IdCRNS);
    $('#IdRoleERNS').val(IdRole);
    $('#IdSkillERNS').val(IdSkill);
    $('#StartERNS').val(StartDate);
    $('#FinishERNS').val(FinishDate);

    if (IdGrade == "NA" || IdGrade == "") {
        $('#IdGradeERNS').val(0);
    }
    else {
        $('#IdGradeERNS').val(IdGrade);
    }
    
    $('#DescERNS').val(Description);
    $('#MainEXPRNS').val(MainExp);
    $('#IdCandidateECRnS').val(IdCandidate); 

    //$('#CandidateName').val(CandidateName);
    //document.getElementById("CandidateName").value = CandidateName;
    var k = 0;
}

function FillAddExpForm(IdCandidate, CandidateName) {
    ShowHideEditFormsCandidates(4);
    $('#CandidateName').val(CandidateName);
    $('#IdCandidate').val(IdCandidate);
    var k = 0;
}

function ShowHideEditFormsCandidates(i){
    $('#CreateCandidateMainExp').hide();
    $('#EditCandidate').hide();
    $('#CreateExp').hide();
    $('#AddExp').hide();
    $('#EditExp').hide();
    $('#AddNote').hide();
    $('#EditNoteForm').hide();
    $('#AddLeadNote').hide();
    $('#EditLeadNoteForm').hide();
    $("#IdCandidateNote").val("");

    if (i == 1) {
        $('#CreateCandidateMainExp').show();
    }
    if (i == 2) {
        $('#EditCandidate').show();
    }
    if (i == 3) {
        $('#CreateExp').show();
    }
    if (i == 4) {
        $('#AddExp').show();
    }
    if (i == 5) {
        $('#EditExp').show();
    }
    if (i == 6) {
        $('#AddNote').show();
    }
    if (i == 7) {
        $('#EditNoteForm').show();
    }
    if (i == 8) {
        $('#AddLeadNote').show();
    }
    if (i == 9) {
        $('#EditLeadNoteForm').show();
    }
}

function fillEditFormNationalities(IdNationality, NationalityDescriptiion, forCandidates, forLeads, Active) {
    $("#oldId").val(IdNationality);
    $("#oldNationality").val(NationalityDescriptiion);
    $("#oldforCandidates").val(forCandidates);
    $("#oldforLeads").val(forLeads);
    $("#oldActive").val(Active);
    
    //var cbActivated = $("#oldActive");
    //if (Active === 1)
    //    $("#Activated").val(Active);
    //else
    //    $("#Activated").val(0);
}

function FillAddNote(item) {
    //$('#');
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////// **CANDIDATE STATUS LISTS** /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function showCandidateStatusList()
{
    $.getJSON("/Adn/JSONCandidateStatusList", showCandidateStatus);
}

function showCandidateStatus(list)
{
    var container = document.getElementById("List");
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>Color</th><th>Color<br/>Code</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        if(item.Color != null){
            var r = item.Color.charAt(0) + item.Color.charAt(1);
            var g = item.Color.charAt(2) + item.Color.charAt(3);
            var b = item.Color.charAt(4) + item.Color.charAt(5);
        }

        table += "<tr><td>";

        table += "<button  onclick='FillEditFormCandidateStatus(" + item.IdStatus + ",\"" + item.Description + "\",\"" + r + "\",\"" + g + "\",\"" + b + "\"," + item.Activated +
            ")'>Edit</button>";

        table += "</td><td> " + item.Description + "</td><td style='background-color:#" + item.Color + "'>    </td><td>#" + item.Color + "</td><td>" + item.Activated + "</td></tr>"
    });

    table += "</table>";

    container.innerHTML = table;
}

function FillEditFormCandidateStatus(IdStatus, Description, R , G , B , Activated)
{
    $('#IdStatus').val(IdStatus);
    $('#eDescription').val(Description);
    $('#eR').val(R);
    $('#eG').val(G);
    $('#eB').val(B);
    $('#Active').val(Activated);
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////// **AVAILABILITY LISTS** ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function showAvailabilityList()
{
    $.getJSON("/Adn/JSONAvailabilityList", showAvailability);
}

function showAvailability(list)
{
    var container = document.getElementById('List');
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>Active</th></tr>";

    $.each(list, function (index, item) {
        table += "<tr><td>";
        table += "<button  onclick='FillEditFormRoleCandidacyAvailabilityLocation(" + item.IdAvailability + ",\"" + item.Description + "\"," + item.Activated + ")'>Edit</button>";
        table +="</td><td>" + item.Description + "</td><td>" + item.Activated + "</td></tr>";
    });

    table += "</table>";
    container.innerHTML += table;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////// **CANDIDACIES LISTS** /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function showCandidaciesList() {
    $.getJSON("/Adn/JSONCandidaciesList", showCandidacies);
}

function showCandidacies(list) {
    var container = document.getElementById('List');
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>Active</th></tr>";

    $.each(list, function (index, item) {
        table += "<tr><td>";
        table += "<button  onclick='FillEditFormRoleCandidacyAvailabilityLocation(" + item.IdCandidacy + ",\"" + item.Description + "\"," + item.Activated + ")'>Edit</button>";
        table += "</td><td>" + item.Description + "</td>";

        if (item.Activated == 1)
            table += "<td>Yes</td>";
        else
            table += "<td>No</td>";
        table += "</tr>";
    });

    container.innerHTML += table;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////// **LOCATION LISTS** ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function showLocationList()
{
    $.getJSON("/Adn/JSONLocationList", showLocation);
}

function showLocation(list)
{
    var container = document.getElementById('List');
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>Active</th></tr>";

    $.each(list, function (index, item) {
        table += "<tr><td>";
        table += "<button  onclick='FillEditFormRoleCandidacyAvailabilityLocation(" + item.IdLocation + ",\"" + item.Description + "\"," + item.Activated + ")'>Edit</button>";
        table += "</td><td>" + item.Description + "</td>";

        if (item.Activated == 1)
            table += "<td>Yes</td>";
        else
            table += "<td>No</td>";
        table += "</tr>";
    });

    container.innerHTML += table;
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////// **CANDIDATE LISTS** ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function SearchCandidate()
{
    var role = $("#Role").val();
    var skill = $("#Skill").val();
    var name = $("#Name").val();
    var nationality = $("#SearchNationality");
    $("#NotesContainer").html("");
    //SearchCandidateClear();

    $.getJSON("/Adn/JSONCandidateList", showCandidates);
}

function showCandidates(list) {

    var role = $("#Role").val();
    var skill = $("#Skill").val();
    var name = $("#Name").val();
    var nationality = $("#SearchNationality").val();
    var optype = $("#OpType").val();
    var ListBy = $("#ListBy").val();

    var candidateListContainer = document.getElementById("List");
    var CandidateInfo = document.getElementById("CandidateInfo");
    var type = document.getElementById("ListType").checked;
    var row = "";
    candidateListContainer.innerHTML = "";
    CandidateInfo.innerHTML = "";
    lista = list;

    ShowHideEditFormsCandidates(100);

    if (ListBy == 1) {
        document.getElementById("ExpList").innerHTML = "";
        $("#Detailed").show();
        $("#ListType").show();
        if (role != "-" && skill == "-" && nationality == "-" && name == "") {
            if (document.getElementById('ListType').checked) {
                row += CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    if (item.IdRole == role) { // ADVANCED
                        row += ListCandidate(item, document.getElementById('ListType').checked);
                    }
                });
            }
            else {
                row += CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    if (item.IdRole == role) {
                        if (item.MainExperience == "1") {
                            row += ListCandidate(item, document.getElementById('ListType').checked);
                        }
                    }
                });
            }
        }
        else if (role == "-" && skill != "-" && nationality == "-" && name == "") {
            if (document.getElementById('ListType').checked) {
                row = CandidateTableHeader(1);
                $.each(list, function (index, item) {
                    if (item.IdSkill == skill) {
                        row += ListCandidate(item, document.getElementById('ListType').checked);
                    }
                });
            }
            else {
                row = CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    if (item.IdSkill == skill) {
                        if (item.MainExperience == "1") {
                            row += ListCandidate(item, document.getElementById('ListType').checked);
                        }
                    }
                });
            }
        }
        else if (role != "-" && skill != "-" && nationality == "-" && optype == "And" && name == "") {
            if (document.getElementById('ListType').checked) {
                row = CandidateTableHeader(1);
                $.each(list, function (index, item) {
                    if (item.IdSkill == skill && item.IdRole == role) {
                        row += ListCandidate(item, document.getElementById('ListType').checked);
                    }
                });
            }
            else {
                row = CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    if (item.IdSkill == skill) {
                        if (item.MainExperience == "1" && item.IdSkill == skill && item.IdRole == role) {
                            row += ListCandidate(item, document.getElementById('ListType').checked);
                        }
                    }
                });
            }
        }
        else if (role != "-" && skill != "-" && nationality == "-" && optype == "Or" && name == "") {
            if (document.getElementById('ListType').checked) {
                row = CandidateTableHeader(1);
                $.each(list, function (index, item) {
                    if (item.IdSkill == skill || item.IdRole == role) {
                        row += ListCandidate(item, document.getElementById('ListType').checked);
                    }
                });
            }
            else {
                row = CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    if (item.IdSkill == skill || item.IdRole == role) {
                        if (item.MainExperience == "1" && (item.IdSkill == skill || item.IdRole == role)) {
                            row += ListCandidate(item);
                        }
                    }
                });
            }
        }
        else if (role == "-" && skill == "-" && nationality != "-" && name == "") {
            if (document.getElementById('ListType').checked) {
                row = CandidateTableHeader(1);
                $.each(list, function (index, item) {
                    if (item.NationalityDescription == nationality) {
                        row += ListCandidate(item, document.getElementById('ListType').checked);
                    }
                });
            }
            else {
                row = CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    if (item.NationalityDescription == nationality) {
                        if (item.MainExperience == "1") {
                            row += ListCandidate(item);                            
                        }
                    }
                });
            }
        }
        if (role == "-" && skill == "-" && nationality == "-" && name != "") {
            row = CandidateTableHeader(1);
            if (document.getElementById('ListType').checked) {
                $.each(list, function (index, item) {
                    var Name = item.Name;
                    name = name.toUpperCase();
                    Name = Name.toUpperCase();
                    if (Name.search(name) != (-1)) {
                        row += ListCandidate(item, document.getElementById('ListType').checked);
                    }
                });
            }
            else {
                row = CandidateTableHeader(0);
                $.each(list, function (index, item) {
                    var Name = item.Name;
                    name = name.toUpperCase();
                    Name = Name.toUpperCase();
                    if (Name.search(name) != (-1)) {
                        if (item.MainExperience == "1") {
                            row += ListCandidate(item, document.getElementById('ListType').checked);
                        }
                    }
                });
            }
            k = "";
        }
        else if (role == "-" && skill == "-" && nationality == "-" && name == "") {
            if (document.getElementById('ListType').checked) {
                row = CandidateTableHeader(1);
                $.each(list, function (index, item) {
                    var valor = document.getElementById('ListType').checked;
                        row += ListCandidate(item, valor);
                });
            }
            else {
                row = CandidateTableHeader(0, ListBy);
                $.each(list, function (index, item) {
                    if (item.MainExperience == "1") {
                        row += ListCandidate(item);
                    }
                });
            }
        }
    }
    else {
        $("#Detailed").hide();
        $("#ListType").hide();
        if (role != "-" && skill == "-" && nationality == "-" && name == "") {
            var sortedStatus = CandidateOrderArray(list);
            row = CandidateTableHeader(0, ListBy);

            var MaxLine = document.getElementById("status").length;
            $.each(sortedStatus, function (index, item) {
                row += "<tr>";
                for (var p = 0; p < MaxLine; p++) {
                    if (typeof item[p] !== 'undefined') {
                        if (item[p].IdRole == role) {
                            var package = item[p];
                            row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                                + item[p].CandidateCode + ' - ' + item[p].Name +
                                '</td>';
                        } else { row += "<td>  </td>"; }
                    } else { row += "<td>  </td>"; }
                }
                row += "</tr>";
            });
        }
        else if (role == "-" && skill != "-" && nationality == "-" && name == "") {
            var sortedStatus = CandidateOrderArray(list);
            row = CandidateTableHeader(0, ListBy);

            var MaxLine = document.getElementById("status").length;
            $.each(sortedStatus, function (index, item) {
                row += "<tr>";
                for (var p = 0; p < MaxLine; p++) {
                    if (typeof item[p] !== 'undefined') {
                        if (item[p].IdSkill == skill) {
                            var package = item[p];
                            row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                                + item[p].CandidateCode + ' - ' + item[p].Name +
                                '</td>';
                        } else { row += "<td>  </td>"; }
                    } else { row += "<td>  </td>"; }
                }
                row += "</tr>";
            });
        }
        else if (role != "-" && skill != "-" && nationality == "-" && optype == "And" && name == "") {
            var sortedStatus = CandidateOrderArray(list);
            row = CandidateTableHeader(0, ListBy);
            var MaxLine = document.getElementById("status").length;
            $.each(sortedStatus, function (index, item) {
                row += "<tr>";
                for (var p = 0; p < MaxLine; p++) {
                    if (typeof item[p] !== 'undefined') {
                        if (item[p].IdRole == role && item[p].IdSkill == skill) {
                            var package = item[p];
                            row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                                + item[p].CandidateCode + ' - ' + item[p].Name +
                                '</td>';
                        } else { row += "<td>  </td>"; }
                    } else { row += "<td>  </td>"; }
                }
                row += "</tr>";
            });
        }
        else if (role != "-" && skill != "-" && nationality == "-" && optype == "Or" && name == "") {
            var sortedStatus = CandidateOrderArray(list);
            row = CandidateTableHeader(0, ListBy);

            var MaxLine = document.getElementById("status").length;
            $.each(sortedStatus, function (index, item) {
                row += "<tr>";
                for (var p = 0; p < MaxLine; p++) {
                    if (typeof item[p] !== 'undefined') {
                        if (item[p].IdRole == role || item[p].IdSkill == skill) {
                            var package = item[p];
                            row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                                + item[p].CandidateCode + ' - ' + item[p].Name +
                                '</td>';
                        } else { row += "<td>  </td>"; }
                    } else { row += "<td>  </td>"; }
                }
                row += "</tr>";
            });
        }
        else if (role == "-" && skill == "-" && nationality != "-" && name == "") {
            var sortedStatus = CandidateOrderArray(list);
            row = CandidateTableHeader(0, ListBy);

            var MaxLine = document.getElementById("status").length;
            $.each(sortedStatus, function (index, item) {
                row += "<tr>";
                for (var p = 0; p < MaxLine; p++) {
                    if (typeof item[p] !== 'undefined') {
                        if (item[p].NationalityDescription == nationality) {
                            var package = item[p];
                            row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                                + item[p].CandidateCode + ' - ' + item[p].Name +
                                '</td>';
                        } else { row += "<td>  </td>"; }
                    } else { row += "<td>  </td>"; }
                }
                row += "</tr>";
            });
        }
        if (role == "-" && skill == "-" && nationality == "-" && name != "") {
            var sortedStatus = CandidateOrderArray(list);
            row = CandidateTableHeader(0, ListBy);

            var MaxLine = document.getElementById("status").length;
            $.each(sortedStatus, function (index, item) {
                row += "<tr>";
                for (var p = 0; p < MaxLine; p++) {
                    if (typeof item[p] !== 'undefined') {
                        var Name = item[p].Name;
                        Name = Name.toUpperCase();
                        name = name.toUpperCase();
                        if (Name.search(name) != (-1)) {
                        var package = item[p];
                        row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                            + item[p].CandidateCode + ' - ' + item[p].Name +
                            '</td>';
                        } else { row += "<td>  </td>"; }
                    } else { row += "<td>  </td>"; }
                }
                row += "</tr>";
            });
        }
        else if (role == "-" && skill == "-" && nationality == "-" && name == "") {

                var sortedStatus = CandidateOrderArray(list);
                row = CandidateTableHeader(0, ListBy);
          
                var MaxLine = document.getElementById("status").length;
                $.each(sortedStatus, function (index, item) {
                    row += "<tr>";

                    for (var p = 0; p < MaxLine; p++) {
                        if (typeof item[p] !== 'undefined') {
                            var package = item[p];
                            row += '<td onclick="candidateSelect(' + package.IdCandidate + ')" style="background-color:#' + item[p].StatusColor + '" >'
                                + item[p].CandidateCode + ' - ' + item[p].Name +
                                '</td>';
                        }
                        else{ row += "<td>  </td>";}
                    }
                    row += "</tr>"; 
                });
            
        } 
    }    
    row += "</table>";
    candidateListContainer.innerHTML += row;
}

function ListCandidate(item, checked) {
    var row = "";
    //$("#CandidateName").val(item.Name);
    document.getElementById("CandidateName").value = item.Name;
    if (checked) {
        row += '<tr style="background-color:#' + item.StatusColor + '">';

        if (k != item.Name) {
            k = item.Name;
            row += "<td><button onclick='FillEditFormCandidate(" + item.IdCandidate + "," + item.IdCRnS + "," + item.IdRolesAndSkills + ",\"" + item.Name + "\"," + item.IdNationality + "," + item.IdRole + "," + item.IdSkill + ",\"" + item.DateStart +
                "\",\"" + item.DateFinish + "\"," + item.Grade + "," + item.IdLocation + "," + item.IdAvailability + ",\"" + item.GrossRemuneration + "\",\"" + item.NET + "\",\"" + item.DailyGains + "\",\"" +
                item.RemunerationNotes + "\"," + item.IdStatus + ",\"" + item.Interview + "\"," + item.IdCandidacy + "," + item.Classification + ",\"" + item.CandidateCode + "\"," + item.MainExperience + "," + item.Activated +
                ",\"" + item.Description + "\",\"" + item.BirthDate + "\""
                + ")'><i class='fas fa-user-edit'></i></button></td>"; // And Main EXP
        } else { row += "<td></td>"; }
        var k = 0;
        row += "<td><button id=" + "'editXP'" + " onclick='FillEditFormExp(" + item.IdRolesAndSkills + "," + item.IdCRnS + "," + item.IdCandidate + "," + item.IdRole + "," + item.IdSkill + ",\"" + item.DateStart + "\",\"" + item.DateFinish + "\",\"" + item.Grade + "\",\"" +
            item.ExpDescription + "\"," + item.MainExperience + ",\"" + item.Name + "\")'><i class='fas fa-file-signature'></i></button></td>"; //Edit EXP
        var j = 0;
        row += "<td><button onclick='FillAddExpForm(" + item.IdCandidate + ",\""+item.Name+"\")'><i class='fas fa-file-contract'></i></button></td>"; //Add EXP

        row += '<td>' + item.Name + '</td><td>' + item.CandidateCode + '</td><td>' + item.NationalityDescription + '</td><td><nobr>' + item.BirthDate + '</nobr></td><td>' + item.Role + '</td><td>' + item.Skill + '</td><td>' + item.SkillType + '</td><td><nobr>' + item.DateStart + '</nobr></nobr></td><td><nobr>' + item.DateFinish +
            '</nobr></td><td>' + item.Grade + '</td><td>' + item.CurrentPlace + '</td><td>' + item.Availability + '</td>';
        if (item.GrossRemuneration == "0") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.GrossRemuneration + '</td>';
        }

        if (item.NET == "0") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.NET + '</td>';
        }

        if (item.DailyGains == "0") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.DailyGains + '</td>';
        }

        row += '<td>' + item.RemunerationNotes + '</td><td>' + item.Status + '</td>';

        if (item.Interview == "") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.Interview + '</td>';
        }

        row += '<td>' + item.Candidacy + '</td>';

        if (item.Classification == 0)
            row += '<td>N/D</td>';
        else
            row += '<td>' + item.Classification + '</td>';

        if (item.MainExperience == 1)
            row += '<td>Yes</td>';
        else
            row += '<td>No</td>';

        //if (item.Active == 0)
        //    row += '<td>No</td>';
        //else
        //    row += '<td>Yes</td>';

        row += '<td style="display:none;">' + item.IdCRnS + '</td ><td style="display:none;">' + item.IdRolesAndSkills + '</td><td style="display:none;">' + item.IdCandidate + '</td>';
        row += '</tr>';
    }
    else {
        row += '<tr style="background-color:#' + item.StatusColor + '">';

        if (k != item.Name) {
            k = item.Name
            row += "<td><button onclick='FillEditFormCandidate(" + item.IdCandidate + "," + item.IdCRnS + "," + item.IdRolesAndSkills + ",\"" + item.Name + "\"," + item.IdNationality + "," + item.IdRole + "," + item.IdSkill + ",\"" + item.DateStart +
                "\",\"" + item.DateFinish + "\"," + item.Grade + "," + item.IdLocation + "," + item.IdAvailability + ",\"" + item.GrossRemuneration + "\",\"" + item.NET + "\",\"" + item.DailyGains + "\",\"" +
                item.RemunerationNotes + "\"," + item.IdStatus + ",\"" + item.Interview + "\"," + item.IdCandidacy + "," + item.Classification + ",\"" + item.CandidateCode + "\"," + item.MainExperience + "," + item.Activated +
                ",\"" + item.Description + "\",\"" + item.BirthDate + "\""
                + ")'><i class='fas fa-user-edit'> </i></button></td>"; //Edit Candidate And Main EXP
        } else { row += "<td></td>";}

        row += "<td><button id=" + "'editXP'" + " onclick='FillEditFormExp(" + item.IdRolesAndSkills + "," + item.IdCRnS + "," + item.IdCandidate + "," + item.IdRole + "," + item.IdSkill + ",\"" + item.DateStart + "\",\"" + item.DateFinish + "\",\"" + item.Grade + "\",\"" +
            item.ExpDescription + "\"," + item.MainExperience + ")'><i class='fas fa-file-signature'></i></button></td>"; //Edit EXP 

        row += "<td><button onclick='FillAddExpForm(" + item.IdCandidate + ",\"" + item.Name +"\")'><i class='fas fa-file-contract'></i></button></td>"; //

        row += '<td>' + item.Name + '</td><td>' + item.CandidateCode + '</td><td>' + item.NationalityDescription + '</td><td><nobr>' + item.BirthDate + '</nobr></td><td>' + item.Role + '</td><td>' + item.Skill + '</td>' +
                        /*<td>' + item.SkillType + '</td>*/'<td><nobr>' + item.DateStart + '</nobr></td><td><nobr>' + item.DateFinish +
            '</nobr></td><td>' + item.Grade + '</td><td>' + item.CurrentPlace + '</td><td>' + item.Availability + '</td>';
        if (item.GrossRemuneration == "0") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.GrossRemuneration + '</td>';
        }

        if (item.NET == "0") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.NET + '</td>';
        }

        if (item.DailyGains == "0") {
            row += '<td><p>N/D</p></td>';
        }
        else {
            row += '<td>' + item.DailyGains + '</td>';
        }

        row += /*'<td>' + item.RemunerationNotes + '</td>'+*/'<td>' + item.Status + '</td>';

        //if (item.Interview == "") {
        //    row += '<td><p>N/D</p></td>';
        //}
        //else {
        //    row += '<td>' + item.Interview + '</td>';
        //}

        //row += '<td>' + item.Candidacy + '</td>';

        if (item.Classification == 0)
            row += '<td>N/D</td>';
        else
            row += '<td>' + item.Classification + '</td>';

        //if (item.MainExperience == 1)
        //    row += '<td>Yes</td>';
        //else
        //    row += '<td>No</td>';

        //if (item.Active == 0)
        //    row += '<td>No</td>';
        //else
        //    row += '<td>Yes</td>';

        row += '<td style="display:none;">' + item.IdCRnS + '</td ><td style="display:none;">' + item.IdRolesAndSkills + '</td><td style="display:none;">' + item.IdCandidate + '</td>';
        row += '</tr>';
    }
    k = item.Name;
    return row;
}

function ListCandidateExpList(target) {
    var table = document.getElementById("ExpList");
    table.innerHTML = "";

    var row = " <table id = 'ExpList' class='CandidateFormat' border = '1' style='width: 100%;'>";
    row +="<tr><th>Edit<br/>Exp</th><th>Role</th><th>Skill</th><th>Type</th><th>Start</th><th>Finish</th><th>Exp<br/>Evaluation</th></tr>";


    $.each(lista, function (index, item) {
        if (item.IdCandidate == target) {
            row += "<tr>";
            row += "<td><button id=" + "'editXP'" + " onclick='FillEditFormExp(" + item.IdRolesAndSkills + "," + item.IdCRnS + "," + item.IdCandidate + "," + item.IdRole + "," + item.IdSkill + ",\"" + item.DateStart + "\",\"" + item.DateFinish + "\",\"" + item.Grade + "\",\"" +
                item.ExpDescription + "\"," + item.MainExperience + ")'><i class='fas fa-file-signature'></i></button></td>"; //Edit EXP
            
            row += "<td>" + item.Role + "</td><td>" + item.Skill + "</td><td>" + item.SkillType + "</td><td><nobr>" + item.DateStart + "</nobr></nobr></td><td><nobr>" + item.DateFinish +
            "</nobr></td><td>" + item.Grade + "</td>"; 
            row += "</tr >";
        }
    });
    row += "</table>";
    table.innerHTML += row;
}

function ListCandidateDescriptions() {
    try {

    } catch (e) {

    }
}

function candidateSelect(target) {
    var item = jQuery.grep(lista, function (item) { return (item.IdCandidate === target) });
    
    //FillEditFormCandidate(item[0].IdCandidate, item[0].IdCRnS, item[0].IdRolesAndSkills, item[0].Name, item[0].IdNationality, item[0].IdRole, item[0].IdSkill, item[0].DateStart,
    //    item[0].DateFinish, item[0].Grade, item[0].IdLocation, item[0].IdAvailability, item[0].GrossRemuneration, item[0].NET, item[0].DailyGains, item[0].RemunerationNotes,
    //    item[0].IdStatus , item[0].Interview , item[0].IdCandidacy , item[0].Classification , item[0].CandidateCode , item[0].MainExperience , item[0].Activated,
    //    item[0].Description, item[0].BirthDate);

   // ShowHideEditFormsCandidates(2);
    GetCandidateInfoTable(target);
    ListCandidateExpList(target);
}

function GetCandidateInfoTable(target) {
    document.getElementById("CandidateInfo").innerHTML = "";

    var row = "<table id = 'Candidate' class='Candidate' border = '1' style='width: 100%;'> <tr>";
    row += "<th>Edit<br/>Cand.</th><th>Add<br/>Exp.</th><th>Name</th><th>Code</th><th>Nat</th><th>Date of<br/>Birth</th><th>Current<br/>Location</th><th>Avail.</th><th>Gross<br/>Salary</th><th>Net<br/>Salary</th><th>Daily<br/>Rate</th>" +
        "<th>Candidate<br/>Status</th><th>Overall<br/>Eval.</th>";
    row += "</tr>";
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].IdCandidate == target) {
            $("#CandidateName").text(lista[i].Name);
            document.getElementById("CandidateName").value = lista[i].Name;
            var k = $("#CandidateName").text();
            var item = lista[i];
            row += "<tr>";
            row += "<td><button onclick='FillEditFormCandidate(" + item.IdCandidate + "," + item.IdCRnS + "," + item.IdRolesAndSkills + ",\"" + item.Name + "\"," + item.IdNationality + "," + item.IdRole + "," + item.IdSkill + ",\"" + item.DateStart +
                "\",\"" + item.DateFinish + "\"," + item.Grade + "," + item.IdLocation + "," + item.IdAvailability + ",\"" + item.GrossRemuneration + "\",\"" + item.NET + "\",\"" + item.DailyGains + "\",\"" +
                item.RemunerationNotes + "\"," + item.IdStatus + ",\"" + item.Interview + "\"," + item.IdCandidacy + "," + item.Classification + ",\"" + item.CandidateCode + "\"," + item.MainExperience + "," + item.Activated +
                ",\"" + item.Description + "\",\"" + item.BirthDate + "\"" +
                ")'><i class='fas fa-user-edit'></i></button></td>"; // And Main EXP";
            row += "<td><button onclick='FillAddExpForm(" + item.IdCandidate + ",\"" + item.Name + "\")'><i class='fas fa-file-contract'></i></button></td>";
            row += "<td>" + item.Name + "</td><td>" + item.CandidateCode + "</td><td>" + item.NationalityDescription + "</td><td>" + item.BirthDate + "</td><td>" + item.CurrentPlace + "</td><td>" + item.Availability + "dias</td>" +
                "<td>" + item.GrossRemuneration + "</td><td>" + item.NET + "</td><td>" + item.DailyGains + "</td><td>" + item.Status + "</td> <td>" + item.Classification + "</td>";
            row += "</tr><tr><th colspan='13'>Description</th></tr><tr>";
            row += "<td colspan='13'>" + item.Description + "</td>";
            document.getElementById("CandidateName").value = item.Name;
            break;
        }
    }
    row += "</tr></table><br/>";

    $("#IdCandidate").val(target);
    document.getElementById("CandidateInfo").innerHTML = row;
    var g = $("#CandidateName").val();
    FillNotes(target);
}

function CreateCandidate() {
    $.ajax({
        type: 'GET',
        url: "/Adn/CreateCandidateFullCVAction",
        async: false,
        data: {
            CreateName: $("#CreateName").val(),
            CreateNationality: $("#CreateNationality").val(),
            Role: $("#CreateRole").val(),
            Init: $("#Init").val(),
            End: $("#End").val(),
            Skill: $("#CreateSkill").val(),
            Desc: $("#Desc").val(),
            Grade: $("#Grade").val(),
            Gross: $("#Gross").val(),
            Eval: $("#Eval").val(),
            avaiability: $("#avaiability").val(),
            DailyGains: $("#DailyGains").val(),
            RemunerationNotes: $("#RemunerationNotes").val(),
            CurrentPlace: $("#CurrentPlace").val(),
            status: $("#status").val(),
            Cand: $("#Cand").val(),
            NET: $("#NET").val(),
            CandidateCode: $("#CandidateCode").val(),
            interview: $("#interview").val(),
            mainxp: $("#mainxp").val(),
            CreateBirthDate: $("#CreateBirthDate").val(),
        /*IdCandidateNote: $("#IdCandidateNote").val(), AddNote: $("#AddNoteText").val(), CandidateName: $("#CandidateName").val() */
            textRole: $("#CreateRole option:selected").text(),
            textSkill: $("#CreateSkill option:selected").text(),
            textPlace: $("#CurrentPlace option:selected").text(),
            textStatus: $("#status option:selected").text(),
            textAvailaility: $("#avaiability option:selected").text(),
            textNat: $("#CreateNationality option:selected").text(),
            textEval: $("#Eval option:selected").text(),
            textGrade: $("#Grade option:selected").text(),
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (exception) {
            alert("ERROR -> Não deu para criar candidate<br />" + exception.responseText);
            SearchCandidate();
            //container.innerHTML = "Failed to get Candidate List";
        },
        success: function (result) {
            //DescriptionList = result;
            SearchCandidate();
        }
    });
}

function EditCandidate() {

    $.ajax({
        type: 'GET',
        url: "/Adn/EDITCandidate",
        async: false,
        data: {
            id: $("#id").val(),
            UpdateName: $("#UpdateName").val(),
            UpdateDesc: $("#UpdateDesc").val(),
            UpdateGross: $("#UpdateGross").val(),
            UpdateAvaiability: $("#UpdateAvaiability").val(),
            UpdateEvaluation: $("#UpdateEvaluation").val(),
            UpdateNET: $("#UpdateNET").val(),
            UpdateDailyGains: $("#UpdateDailyGains").val(),
            UpdateRemunerationNotes: $("#UpdateRemunerationNotes").val(),
            UpdateCurrentPlace: $("#UpdateCurrentPlace").val(),
            UpdateInterview: $("#UpdateInterview").val(),
            UpdateStatus: $("#UpdateStatus").val(),
            UpdateCand: $("#UpdateCand").val(),
            UpdateCandidateCode: $("#UpdateCandidateCode").val(),
            EditNationality: $("#EditNationality").val(),
            EditBirthDate: $("#EditBirthDate").val(),
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (exception) {
            alert("ERROR -> Não deu para editar candidate<br />" + exception.responseText);
            SearchCandidate();
            //container.innerHTML = "Failed to get Candidate List";
        },
        success: function (result) {
            //DescriptionList = result;
            SearchCandidate();
        }
    });
}

function AddExp() {
    var k = $("#IdCandidate").val();
    var a = $("#IdRoleAddExp").val();
    var b = $("#IdSkillAddExp").val();
    var c = $("#StartAddExp").val();
    var d = $("#FinishAddExp").val();
    var e = $("#IdGradeAddExp").val();
    var f = $("#DescAddExp").val();
    var g = $("#MainEXPAddExp").val();
    var h = $("#CandidateName").val();
    var i = $("#IdRoleAddExp option:selected").text();
    var j = $("#IdSkillAddExp option:selected").text();
    $.ajax({
        type: 'GET',
        url: "/Adn/AddExp",
        async: false,
        data: {
            IdCandidateAddExp: $("#IdCandidate").val(),
            IdRoleAddExp: $("#IdRoleAddExp").val(),
            IdSkillAddExp: $("#IdSkillAddExp").val(),
            StartAddExp: $("#StartAddExp").val(),
            FinishAddExp: $("#FinishAddExp").val(),
            IdGradeAddExp: $("#IdGradeAddExp").val(),
            DescAddExp: $("#DescAddExp").val(),
            MainEXPAddExp: $("#MainEXPAddExp").val(),
            CandidateName: $("#CandidateName").val(),
            newRole: $("#IdRoleAddExp option:selected").text(),
            newSkill: $("#IdSkillAddExp option:selected").text(),
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (exception) {
            alert("ERROR -> Não deu para editar candidate<br />" + exception.responseText);
            SearchCandidate();
            //container.innerHTML = "Failed to get Candidate List";
        },
        success: function (result) {
            //DescriptionList = result;
            SearchCandidate();
        }
    });
}

function EditExp() {

    var k = $("#CandidateName").val();
    var a = $("#IdSkillERNS option:selected").text();
    var b = $("#IdRoleERNS option:selected").text();
    var c = $("#IdGradeERNS option:selected").text();
    var ds = $("#MainEXPRNS option:selected").val();
    var o = 0;
    $.ajax({
        type: 'GET',
        url: "/Adn/EditExp",
        async: false,
        data: {
            IdRnSERNS: $("#IdRnSERNS").val(),
            IdECRnS: $("#IdECRnS").val(),
            IdCandidateECRnS: $("#IdCandidateECRnS").val(),
            IdRoleERNS: $("#IdRoleERNS").val(),
            IdSkillERNS: $("#IdSkillERNS").val(),
            StartERNS: $("#StartERNS").val(),
            FinishERNS: $("#FinishERNS").val(),
            IdGradeERNS: $("#IdGradeERNS").val(),
            DescERNS: $("#DescERNS").val(),
            MainEXPRNS: $("#MainEXPRNS option:selected").val(),
            textSkill: $("#IdSkillERNS option:selected").text(),
            textRole: $("#IdRoleERNS option:selected").text(),
            textGrade: $("#IdGradeERNS option:selected").text(),
            CandidateName: $("#CandidateName").val(),
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (exception) {
            alert("ERROR -> Não deu para editar candidate<br />" + exception.responseText);
            SearchCandidate();
            //container.innerHTML = "Failed to get Candidate List";
        },
        success: function (result) {
            //DescriptionList = result;
            SearchCandidate();
        }
    });
}

function FillNotes(target) {
    var row = "<button onclick='FillAddNote(\"" + target + "\")'>Add Note</button>";
    row += "<table border='1'><tr><th colspan='5'>Notes</th></tr >";
    $.each(DescriptionList, function (index, item) {
        var k = 0;
        if (item.IdCandidate == target && item.Active == 1) {
            row += "<tr><td><button onclick='FillEditNote(" + item.IdCandidateDescription + ",\"" + item.Note + "\")'>edit</button></td><td><button onclick='DeleteCandidateNote(" + item.IdCandidateDescription + "," +
                item.IdCandidate + ",\"" + item.Note + "\",\"" + item.CandidateName + "\")'>delete</button></td>" +
               "<td><label>" + item.User + "</label></td > <td><label>" +
                item.TimeStamp + "</label></td><td><label>" + item.Note + "</label></td></tr> ";
        }
    });
    row += "</table><br />"; 
    document.getElementById("NotesContainer").innerHTML = row;
}

function FillAddNote(IdCandidate)
{
    ShowHideEditFormsCandidates(6);
    $("#IdCandidateNote").val(IdCandidate);
}

function FillEditNote(IdNote, note) {
    ShowHideEditFormsCandidates(7);
    $("#IdNoteEdit").val(IdNote);
    var area = document.getElementById("EditNote");
    $("#OldNote").val(note);
    area.value = note;
}

function CreateCandidateNote() {
    $.ajax({
        type: 'POST',
        url: '/Adn/AddNoteAction',
        data: { IdCandidateNote: $("#IdCandidateNote").val(), AddNote: $("#AddNoteText").val(), CandidateName: $("#CandidateName").val()},
        success: function (result) {
            $("#AddNote").hide();
            if (result == "True") {
                GetCandidateNotes();
                FillNotes($("#IdCandidate").val());
            }
            else {
                alert("Ocurreu um erro.");
                GetCandidateNotes();
                FillNotes(IdCandidate);
            }
        }
    });
}

function UpdateCandidateNote() {
    $.ajax({
        type: 'POST',
        url: '/Adn/EditCandidateNoteAction',
        data: { IdNoteEdit: $("#IdNoteEdit").val(), EditNote: $("#EditNote").val(), OldNote: $("#OldNote").val(), CandidateName: $("#CandidateName").val()},
        success: function (result) {
            $("#EditNoteForm").hide();
            if (result == "True") {
                GetCandidateNotes();
                FillNotes($("#IdCandidate").val());
                //FillNotes(IdCandidate);
            }
            else {
                alert("Ocurreu um erro.");
            }
        }
    });
}

function DeleteCandidateNote(IdCandidateDescription, IdCandidate, OldNote, CandidateName) {
    $.ajax({
        type: 'POST',
        url: '/Adn/DeleteNoteAction',
        data: { IdNote: IdCandidateDescription, OldNote: OldNote, CandidateName: CandidateName },
        success: function (result) {
            if (result == "True") {
                GetCandidateNotes();
                FillNotes(IdCandidate);
            }
            else {
                alert("Ocorreu um erro.");
            }
        }
    });
}

 //Get Descriptions from 
function GetCandidateNotes() {

    var CandidateNotes = {
        IdCandidateDescription: 0,
        IdCandidate: 0,
        Note: "",
        TimeStamp: "",
        Active: 0,
        IdUser: 0,
        User: "",
    };
    
    $.ajax({
        type: 'GET',
        url: "/Adn/JSONGetCandidateNotes",
        async: false,
        data: JSON.stringify(CandidateNotes),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (e) {
            alert("ERROR -> Não deu a busca de notes <br/>" + e.responseText);
            //container.innerHTML = "Failed to get Candidate List";
        },
        success: function (result) {
            DescriptionList = result;
            //SearchCandidate();
        }
    });
}

function GetCandidateExp(IdCand) {
    var package = [];
    $.each(lista, function (idex, item) { if (item.IdCand === IdCand) { package.push(item); } });
    return package;
}

function transpose(matrix, l, c) {

    var result = [];
    if (document.getElementById("status"))
        var nStatus = document.getElementById("status").length;
    else
        var nStatus = document.getElementById("editLeadStatus").length;

    for (var i = 0; i < l; i++) {
        result[i] = [];
    }

    for (var i = 0; i < c; i++) { 
        for (var j = 0; j < l; j++) {
             if (matrix[i][j] != null) {
                var box = matrix[i][j];
                result[j][i] = matrix[i][j];
            } 
        }
    }
    
    return result;
}

function matrixMaxCol(matrix) {
    var MaxLine = 0;
    var nStatus = document.getElementById("status").length;
    for (var c = 0; c < nStatus; c++) {
        if (MaxLine < matrix[c].length)
            MaxLine = matrix[c].length;
    }
    return MaxLine;
}

function CandidateOrderArray(list) {
    var CandidateList = [];

//determine the maximum number os candidate status
    if (document.getElementById("status")) {
        var nStatus = document.getElementById("status").length;
    } else {
        var nStatus = document.getElementById("editLeadStatus").length;
    }
        
//determine the maximum number of candidates in the same candidate status
    
    
//create the bidimensional table with the candidates in each HR status
    for (var i = 0; i < nStatus; i++) {
        CandidateList[i] = [];
    }
    
     //mudar a incerção de candidatos no array para algo dinamico. (meter idstatus dentro de um array e mudar aquilo  para um ou dois fors)
    //NÃO ESQUECER na filtragem dos candidatos, é preciso procurar skills e roles na lista de exp.
    if (document.getElementById('status')) {
        var IdStatusArray = document.getElementById('status').options;
        for (var i = 0; i < list.length; i++) { //it iterates the list of exp
            if (list[i].MainExperience == "1") { // it checks if exp is main
                for (var j = 0; j < IdStatusArray.length; j++) { //it iterates through the states id array
                    if (list[i].IdStatus == IdStatusArray[j].value) {
                        CandidateList[j].push(list[i]);
                    };
                }
            };
        }
    } else {
        var IdStatusArray = document.getElementById('editLeadStatus').options;
        for (var i = 0; i < list.length; i++) { //it iterates the list of exp
           
                for (var j = 0; j < IdStatusArray.length; j++) { //it iterates through the states id array
                    if (list[i].IdLeadStatus == IdStatusArray[j].value) {
                        CandidateList[j].push(list[i]);
                    };
                }
           
        }
    }

    var l = getLineMax(CandidateList);
    
    var result = transpose(CandidateList, l, nStatus);

    return result;
}

function getLineMax(CandidateList) {
    var MaxLine = 0;

    for (var c = 0; c < CandidateList.length; c++) {
        if (MaxLine < CandidateList[c].length)
            MaxLine = CandidateList[c].length;
    }

    return MaxLine;
}

function CandidateTableHeader(type) {
    var box = "";
    var ListBy = $("#ListBy").val();
    //ListBy shows the candidate list by status or by name
    if (ListBy == 0) { //show the candidate list by status
        box = " <table id = 'AdvStatus' class='CandidateFormat' border = '1' style='width: 100%;'> <tr>";

        var teste = [];
        $("#status option").each(function (index, item) {
            teste.push(item.textContent);
            box += "<th>" + item.textContent + "</th>";
        });

        box += "</tr>";
    }
    else if (ListBy == 1) //show the candidate list by name
    {
        if (type == 0) {//summarized
            box = "<table id='AdvList' class='CandidateFormat' border='1'><tr>" +
                "<th>Edit<br/>Cand.</th><th>Edit<br/>Exp.</th><th>Add<br/>Exp.</th><th>Name</th><th>Candidate<br/>Code</th><th>Nat.</th><th>Date of<br/>Birth</th><th>Role</th><th>Skill</th>" +/*"<th>Skill<br />Type</th>"+*/
                "<th>Start<br />Date</th><th>Finish<br />Date</th><th>Skill<br />Eval.</th>" + "<th>Current<br />location</th><th>Avail.</th><th>Gross<br />Salary</th>" +
                "<th>Net<br />Salary</th><th>Daily<br />Rate</th>" + /*"<th>Remuneration<br/>Notes</th>"*/ "<th>Candidate<br />Status</th>"/*<th>Interview<br />Date</th>*//*"<th>Candidature<br />Source</th>"*/ +
                "<th>Overall<br />Eval.</th>" +/*<th>Main <br />Experience</th>*//*"<th>Activated</th>"*/
                "<th style='display:none;'>@Html.DisplayNameFor(model => model.IdCRnS)</th><th style='display:none;'>@Html.DisplayNameFor(model => model.IdRolesAndSkills)</th><th style='display:none;'>@Html.DisplayNameFor(model => model.IdCandidate)</th>" +
                "</tr>";
        }
        else {//advanced detail
            box = "<table id='AdvList' class='CandidateFormat' border='1'><tr>" +
                "<th>Edit<br/>Cand.</th><th>Edit<br/>Exp.</th><th>Add<br/>Exp.</th><th>Name</th><th>Candidate<br/>Code</th><th>Nat.</th><th>Date of<br/>Birth</th><th>Role</th><th>Skill</th><th>Skill<br />Type</th><th>Start<br />Date</th><th>Finish<br />Date</th><th>Skill<br />Eval.</th>" +
                "<th>Current<br />location</th><th>Avail.</th><th>Gross<br />Salary</th>" +
                "<th>Net<br />Salary</th><th>Daily<br />Rate</th>" + "<th>Salary<br/>Notes</th><th>Candidate<br />Status</th><th>Interview<br />Date</th><th>Candidature<br />Source</th>" +
                "<th>Overall<br />Eval.</th><th>Main <br />Exp.</th>" +
                "<th style='display:none;'>@Html.DisplayNameFor(model => model.IdCRnS)</th><th style='display:none;'>@Html.DisplayNameFor(model => model.IdRolesAndSkills)</th><th style='display:none;'>@Html.DisplayNameFor(model => model.IdCandidate)</th>" +
                "</tr>";;
        }
    }

    return box;
}

function SearchCandidateClear() {
    $("#Role").val("-");
    $("#Skill").val("-");
    $("#SearchNationality").val("-");
    $("#Name").val("");
    //$("#NotesContainer").innerHTML("");
    $("#NotesContainer").hide();
    SearchCandidate();
}

///////////////////////////////////////////////////////////////////////////
///////////////////////LogIn///////////////////////////////////////////////
function LogInCheck()
{
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        var details = $('#loginForm').serialize();
        $.post('');
    });
}

//////////////////////////////////////////////////////////////////////////
/////////////////////Users////////////////////////////////////////////////
function showUserList() {
    $.getJSON("/Adn/JSONGetUsersList", showUsers);
}

function showUsers(list) {
    var container = document.getElementById("List");
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Name</th><th>Role</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        table += "<tr><td>";

        table += "<button  onclick='FillEditUserForm(\"" + item.Name + "\",\"" + item.Password + "\"," + item.Activated + "," + item.IdUser + "," + item.IdUserRole +
            ")'>Edit</button>";

        table += "</td><td> " + item.Name + "</td><td>" + item.UserRoleDescription + "</td><td>";
        if (item.Activated == 1)
            table += "Yes";
        else
            table += "No";
        table += "</td></tr>"
        $("#CandidateName").val(item.Name);
    });

    table += "</table>";
    container.innerHTML = table;
}

function FillEditUserForm(name, password, active, IdUser, IdRole) {
    var i = 0;
    $("#editName").val(name);
    $("#editPass").val(password);
    $("#Activated").val(active);
    $("#IdUser").val(IdUser);
    $("#EditIdCRole").val(IdRole);
}

function CreateUser()
{
    var i = 0;
    var k = $("#IdCRole option:selected").val();
    $.ajax({
        type: 'POST',
        url: '/Adn/CreateUserAction',
        data: { Name: $("#Name").val(), Password: $("#Password").val(), IdCRole: $("#IdCRole option:selected").val(), CreateRoleDescription: $("#IdCRole option:selected").text() },
        success: function (result) {
            if (result == "True") {
                showUserList();
            }
            else {
                alert("Ocurreu um erro.");
                showUserList();
            }
        }
    });
}

function EditUser(){
    var r = $("#EditIdCRole option:selected").val();
    var i = 0;
    $.ajax({
        type: 'POST',
        url: '/Adn/EditUserAction',
        data: {
            IdUser: $("#IdUser").val(), editName: $("#editName").val(), editPass: $("#editPass").val(), EditIdCRole: $("#EditIdCRole option:selected").val(),
            Activated: $("#Activated option:selected").val(), RoleText: $("#EditIdCRole option:selected").val()
        },
        success: function (result) {
            if (result == "True") {
                showUserList();
            }
            else {
                alert("Ocurreu um erro.");
                showUserList();
            }
        }
    });
}

////////////////////////////////////////////////////////////////////////////
/////////////////////////////UserRoles//////////////////////////////////////
function showUserRoles() {
    var list = $.getJSON("/Adn/JSONGetUserRolesList",showUserRolesList);
}

function showUserRolesList(list) {

    var container = document.getElementById("List");
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>WP</th><th>Access</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        table += "<tr><td>";

        table += "<button  onclick='FillEditFormCandidateStatus(" + item.Description + ",\"" + item.WPCode + ",\"" + item.TypeOfAccess + ",\"" + item.Active +")'>hello</button>";

        table += "</td><td> " + item.Description + "</td><td>" + item.WPCode + "</td><td>" + item.TypeOfAccess +"<td>";
        if (item.Activated == 1)
            table += "Yes";
        else
            table += "No";
        table += "</td></tr>"
    });

    table += "</table>";

    container.innerHTML = table;

}

////////////////////////////////////////////////////////////////////////////
/////////////////////////////WP////////////////////////////////////////////
function showWP() {
    var list = $.getJSON("/Adn/JSONGetWorkPackagesList", showWPList);
}

function showWPList(list) {

    var container = document.getElementById("List");
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Code</th><th>WorkPackage</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        table += "<tr><td>";

        table += "<button  onclick='FillEditFormCandidateStatus(" + item.IdWP + ",\"" + item.WPCode + ",\"" + item.Description + ",\")'>hello</button>";

        table += "</td><td> " + item.WPCode + "</td><td>" + item.Description + "</td><td>";
        if (item.Active == 1)
            table += "Yes";
        else
            table += "No";
        table += "</td></tr>"
    });

    table += "</table>";

    container.innerHTML = table;
}

////////////////////////////////////////////////////////////////////////////
//////////////////////////Customers/////////////////////////////////////////
function showCustomers() {
    var list = $.getJSON("/Adn/JSONGetCustomersList", showCustomersList);
}

function showCustomersList(list) {
    var container = document.getElementById("List");  
    container.innerHTML = "";
    var table = "<table border='1'><tr><th>Action</th><th>Client</th><th>NIF</th><th>Legal<br/>Representative</th><th>Adress</th><th>Email</th><th>FinancialOrProcurementDepartmentEmail</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        table += "<tr><td>";

        table += "<button  onclick='FillEditFormCustomers(" + item.IdCustomer+",\"" + item.ClientName + "\"," + item.NIF + ",\"" + item.LegalRepresentative + "\",\"" + item.Adress + "\",\"" + item.Email + "\",\"" + item.FinancialOrProcurementDepartmentEmail+"\","+item.Active+")'>Edit</button>";

        table += "</td><td> " + item.ClientName + "</td><td>" + item.NIF + "</td><td>" + item.LegalRepresentative + "</td><td>" + item.Adress + "</td><td>" + item.Email + "</td><td>" + item.FinancialOrProcurementDepartmentEmail + "</td><td>";
        if (item.Active == 1)
            table += "Yes";
        else
            table += "No";
        table += "</td></tr>"
    });
    table += "</table>";

    container.innerHTML = table;
}

function FillEditFormCustomers(IdCustomers, ClientName, NIF, LegalRepresentative, Adress, Email, FinancialDepartmentEmail, Active) {
    $("#oldId").val(IdCustomers);
    $("#oldClientName").val(ClientName);
    $("#oldNIf").val(NIF);
    $("#oldLegalRepresentative").val(LegalRepresentative);
    $("#oldAdress").val(Adress);
    $("#oldEmail").val(Email);
    $("#oldFinancialDepartmentEmail").val(FinancialDepartmentEmail);

    var cbActivated = $("#oldActive");
    if (Active === 1)
        $("#oldActive").val(1);
    else
        $("#oldActive").val(0);
}
////////////////////////////////////////////////////////////////////////////
/////////////////////////////Leads//////////////////////////////////////////
function showhideForms(type) {
    switch (type) {
        case 0:
            $("#Info").show();
            $("#FormCreate").hide();
            $("#FormEdit").hide();            
            break;
        case 1:
            $("#Info").hide();
            $("#FormCreate").show();
            $("#FormEdit").hide();
            break;
        case 2:
            $("#Info").hide();
            $("#FormCreate").hide();
            $("#FormEdit").show();
            break;
        default:
            $("#Info").hide();
            $("#FormCreate").hide();
            $("#FormEdit").hide();
            break;
    }
}

function showLeads() {
    //var list = $.getJSON("/Adn/JSONGetLeadsList", showLeadsList);
    $.getJSON("/Adn/JSONGetLeadsList", showLeadsList);
}

function showLeadsList(list) {
    lista = list;
    var container = document.getElementById("List");
    container.innerHTML = "";

    var row = LeadsTableHeader();
    container.innerHTML = row;
    var teste = CandidateOrderArray(list);
    var sortedStatus = CandidateOrderArray(list);
    var MaxLine = document.getElementById("editLeadStatus").length;

    $.each(sortedStatus, function (index, item) {
        row += "<tr>";

        for (var p = 0; p < MaxLine; p++) {
            if (typeof item[p] !== 'undefined') {
                var teste = item[p];
                row += '<td onclick="ShowLeadInfo(' + item[p].IdLead + /*',' + item[p].LeadCode + ',' + index+','+p+*/')" style="background-color:#' + item[p].LeadStatusColor + '" >'
                    + item[p].ClientName + ' - ' + item[p].LeadCode +
                    '</td>';
            }
            else { row += "<td>  </td>"; }
        }
        row += "</tr>";
    });

    row += "</table>";
    container.innerHTML = row;
    showhideForms(-1);
}

function ShowLeadInfo(id) {
    var content = document.getElementById("Info");
    content.innerHTML = "";
    document.getElementById("CandidateResults").innerHTML = "";
    var box = "";
    box = "<table class='CandidateFormat' border = '1' style='width: 100%;'>";
    box += "<tr><th>Edit<br/>Lead</th><th>Lead<br>Code</th><th>Client</th><th>Role</th><th>Role<br/>Exp</th><th>Skill</th><th>Skill<br/>Exp</th><th>Nat</th><th>Max<br/>Exp</th><th>Min<br/>Exp</th><th>Max<br/>Rem</th>" +
        "<th>Min<br/>Rem</th><th>Max<br/>Age</th><th>Min<br/>Age</th><th>Availability</th></tr>" +
        "<tr><th></th></tr > ";
    $.each(lista, function (index, item) {
        if (item.IdLead == id) {
            box += "<td><button onclick='fillEditLeadForm(" + item.IdLead + ")'>Edit Lead</button></td>" + "<td id='InfoLeadCode'>" + item.LeadCode + "</td><td>" + item.ClientName + "</td><td id='Role'>" + item.NameRole + "</td><td>";

            if (item.RoleExp == 0)
                box += "";
            else
                box += item.RoleExp;

            box += "</td><td id='Skill'>" + item.NameSkill + "</td><td>";
            if (item.SkillExp == 0)
                box += "";
            else
                box += item.SkillExp;

            box += "</td><td id='LeadNationality'>" + item.NationalityDescription + "</td><td>";
            if (item.MaxProfessionalExp == 0)
                box += "";
            else
                box += item.MaxProfessionalExp;

            box += "</td><td>";
            if (item.MinProfessionalExp == 0)
                box += "";
            else
                box += item.MinProfessionalExp;

            box +="</td><td id='LeadMaxRem'>";
            if (item.MaxRemuneration == 0)
                box += "";
            else
                box += item.MaxRemuneration;

            box += "</td><td id='LeadMinRem'>";
            if (item.MinRemuneration == 0)
                box += "";
            else
                box += item.MinRemuneration;

            box += "</td><td>";
            if (item.MaxAge == 0)
                box += "";
            else
                box += item.MaxAge;

            box += "</td><td>";
            if (item.MinAge == 0)
                box += "";
            else
                box += item.MinAge;

            box += "</td><td>" + item.AvailabilityDescription + "days</td></tr>";
            box += "<tr><th colspan='15'>Description</th></tr>";
            box += "<tr><td colspan='15'>" + item.Description + "</td></tr>";
        }
    });
    box += "</table><br /><button onclick='ShowHideEditFormsCandidates(8)'>Add Note</button><br />";
    content.innerHTML = box;
    content.innerHTML += "<button onclick='searchCandidates()'>Search for Candidates</button>";
    //$("#IdLead").val(id);
    document.getElementById("IdLeadSafe").value = id;
    ListLeadNotes(id);
    showhideForms(0);
}

function GetLeadNotes() {
    var LeadNotes = {
        IdLeadNote: 0,
        IdLead: 0,
        Description: "",
        TimeStamp: "",
        Active: 0,
        IdUser: 0,
        User: "",
    };

    $.ajax({
        type: 'GET',
        url: "/Adn/JSONGetLeadsNotes",
        async: false,
        data: JSON.stringify(LeadNotes),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (e) {
            alert("ERROR -> Não deu a busca de notes <br/>" + e.responseText);
        },
        success: function (result) {
            DescriptionList = result;
        }
    });
}

function fillEditLeadForm(IdLead) {

    $.each(lista, function (index, item) {
        if (item.IdLead == IdLead) {
            $("#IdLead").val(item.IdLead);
            $("#editLeadCustomer").val(item.IdCustomer); 
            $("#editLeadCode").val(item.LeadCode);
            $("#editLeadSkill").val(item.IdSkill);
            $("#editLeadSkillEXP").val(item.SkillExp);
            $("#editLeadRole").val(item.IdRole);
            $("#editLeadRoleExp").val(item.RoleExp);
            $("#editLeadMinXP").val(item.MinProfessionalExp);
            $("#editLeadMaxXP").val(item.MaxProfessionalExp);
            $("#editLeadMinAge").val(item.MinAge);
            $("#editLeadMaxAge").val(item.MaxAge);
            $("#editLeadMinSal").val(item.MinRemuneration);
            $("#editLeadMaxSal").val(item.MaxRemuneration);
            $("#editLeadNationality").val(item.IdNationality);
            $("#editLeadAvailability").val(item.IdAvailability);
            $("#editLeadStatus").val(item.IdLeadStatus);
            $("#editDescription").val(item.Description);
        }
    });

    showhideForms(2);
}

function LeadsTableHeader() {
    var box = "<table id = 'LeadStatus' class='CandidateFormat' border = '1' style='width: 100%;'> <tr>";

    var teste = [];
    $("#editLeadStatus option").each(function (index, item) {
        teste.push(item.textContent);
        box += "<th>" + item.textContent + "</th>";
    });

    box += "</tr>";
    return box;
}

function ListLeadNotes(id) {
    var box = "<table border='1'><tr><th colspan='5'>Notes</th></tr>";

    $.each(DescriptionList, function (index, item) {
        if (item.IdLead == id && item.Active == 1) {
            box += '<tr><td><button onclick=\"FillEditLeadNoteForm(\'' + item.Description + '\',\'' + item.IdLeadNote + '\')\">Edit</button></td><td><button onclick=\"DeleteNote(' + item.IdLeadNote +
                ',\'' + item.Description + '\')\">Delete</button></td>' + '<td><label>' + item.User + '</label></td><td><label>' + item.TimeStamp + '</label></td> <td><label>' + item.Description + '</label></td></tr > ';
        };
    });
    box += "</table>";
    //$("#NotesContainer").innerHTML = box;
    document.getElementById("NotesContainer").innerHTML = box;
}

function CreateLeadNote() {
    $.ajax({
        type: 'POST',
        url: '/Adn/CreateLeadNoteAction',
        data: { IdLead: $("#IdLeadSafe").val(), AddNoteText: $("#AddNoteText").val() },
        success: function (result) {
            $("#AddLeadNote").hide();
            if(result == "True")
            if (result == "True") {
                GetLeadNotes();
                ListLeadNotes($("#IdLeadSafe").val());
            }
            else {
                alert("Ocurreu um erro.");
                GetCandidateNotes();
                FillNotes(IdCandidate);
            }
        }
    });
}

function FillEditLeadNoteForm(Description, IdLeadNote) {
    $("#IdLeadNote").val(IdLeadNote);
    $("#EditNote").val(Description);
    $("#OldNote").val(Description);

    ShowHideEditFormsCandidates(9);
}

function EditLeadNote() {
    $.ajax({
        type: 'POST',
        url: '/Adn/EditLeadNoteAction',
        data: { IdNote: $("#IdLeadNote").val(), EditNote: $("#EditNote").val(), OldNote: $("#OldNote").val()},
        success: function (result) {
            $("#EditLeadNoteForm").hide();
            GetLeadNotes();
            ListLeadNotes($("#IdLeadSafe").val());
        },
        failed: function (result) { alert(result); },
    });
}

function DeleteNote(idLead, oldNote) {
    var i = 0; 
     $.ajax({
        type: 'POST',
        url: '/Adn/DeleteLeadNoteAction',
        data: { IdLeadNote: idLead, oldNote: oldNote },
        success: function (result) {
            $("#EditLeadNoteForm").hide();
            if (result == "True") {
                GetLeadNotes();
                ListLeadNotes($("#IdLeadSafe").val());
            } else alert("Aconteceu um erro. A nota não foi apagada.");
        },
        failed: function (result) { alert(result); },
    });
}

function searchCandidates()
{
    var cls = GetCandidateLeadStatus();
    var Candidates = {
        IdCRnS:0,
        IdCandidate:0,
        Name:"",
        IdRolesAndSkills: 0,
        IdRole: 0,
        Role: "",
        IdSkill: 0,
        Skill: "",
        SkillType: 0,
        DateStart: 0,
        DateFinish: 0,
        Description: "",
        ExpDescription: "",
        Grade: 0,
        GrossRemuneration: 0,
        IdAvailability: 0,
        Availability: "",
        Classification: 0,
        Activated: 0,
        UserEvaluation: 0,
        NET: 0,
        DailyGains: 0,
        RemunerationNotes: "",
        CurrentPlace: "",
        IdLocation: 0,
        IdStatus: 0,
        Status: "",
        StatusColor: "",
        Interview: 0,
        Candidacy: "",
        IdCandidacy: 0,
        MainExperience: 0,
        CandidateCode: 0,
        IdNationality: 0,
        NationalityDescription: "",
        BirthDate: 0,
        CandidateDescription: "",
    };

    var container = document.getElementById("CandidateResults");
    var role = document.getElementById("Role").innerText;
    var skill = document.getElementById("Skill").innerText;
    var table = "<table border='1'>";
    table += LeadCandidateHeader(table);
    try {
        //gets a list of candidates from the server
        $.ajax({
            type: 'GET',
            url: "/Adn/JSONCandidateList",
            async: false,
            data: JSON.stringify(Candidates),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (e) {
                //alert("ERROR -> " + e.responseText);
                container.innerHTML = "Failed to get Candidate List";
            },
            success: function (result) {
                $.each(result, function (index, item) {
                    if ((role != null && role != " ") && (skill != null && skill != " ")) {
                        if (item.Role == role && item.Skill == skill) {
                            //table += ListCandidate(item, false);
                            table = ListCandidatesLeadVer(table, item, cls);
                        }
                    } else if ((role != " ") && (skill == " ")) {
                        if (item.Role == role) {
                            //table += ListCandidate(item, false);
                            table = ListCandidatesLeadVer(table, item, cls);
                        }
                    } else if ((role == " ") && (skill != " ")) {
                        if (item.Skill == skill) {
                            //table += ListCandidate(item, false);
                            table = ListCandidatesLeadVer(table, item, cls);
                        }
                    }
                });
                table += "</table>";
                container.innerHTML = table;
            }
        });

        //Get a list of LeadsCandidates
        $.getJSON("/Adn/JsonGetLeadsCandidatesList", function (result) {
            $.each(result, function (index, item) {
                $("#cls" + item.IdCandidate).val(item.IdCandidateLeadStatus);
            });
        });
    } catch (err) {
        alert("A problem has appeared. Please inform the developer of this occurrence. Have a nice day :)");
    }    
}

function LeadCandidateHeader(table) {
    table += "<tr><th>Name</th><th>Code</th><th>Nat</th><th>Birthday</th><th>Role</th><th>Skill</th><th>Start</th><th>Finish</th><th>Skill<br/>Grade</th><th>Location</th><th>Availability</th><th>Gross<br/>Salary</th>"+
        "<th>NET<br/>Salary</th><th>Daily<br/>Rate</th><th>Candidate<br/>Status</th><th>Overall<br/>Eval.</th></tr>";

    return table;
}

function ListCandidatesLeadVer(row, item, cls) {
    row += '<tr style="background-color:#' + item.StatusColor + '">';

    row += '<td>' + item.Name + '</td><td>' + item.CandidateCode + '</td><td>' + item.NationalityDescription + '</td><td><nobr>' + item.BirthDate + '</nobr></td><td>' + item.Role + '</td><td>' + item.Skill + '</td>' +
                        '<td><nobr>' + item.DateStart + '</nobr></td><td><nobr>' + item.DateFinish +
        '</nobr></td><td>' + item.Grade + '</td><td>' + item.CurrentPlace + '</td><td>' + item.Availability + '</td>';
    if (item.GrossRemuneration == "0") {
        row += '<td><p>N/D</p></td>';
    }
    else {
        row += '<td>' + item.GrossRemuneration + '</td>';
    }

    if (item.NET == "0") {
        row += '<td><p>N/D</p></td>';
    }
    else {
        row += '<td>' + item.NET + '</td>';
    }

    if (item.DailyGains == "0") {
        row += '<td><p>N/D</p></td>';
    }
    else {
        row += '<td>' + item.DailyGains + '</td>';
    }

    row +='<td>' + item.Status + '</td>';

    if (item.Classification == 0)
        row += '<td>N/D</td>';
    else
        row += '<td>' + item.Classification + '</td>';

    row += "<td><select id='cls" + item.IdCandidate + "' onchange='ChangeCandidateCLS(" + item.IdCandidate + ")'>";
    row +="<option value='-1'>-</option>"
    $.each(cls, function (index, kitem) {
        row += "<option value=" + kitem.IdCandidateLeadStatus + ">" + kitem.Description + "</option>";
    });
    row+="</select></td>";

    var array = [item.CandidateCode, item.Name, item.NationalityDescription, item.Role, item.Skill, item.GrossRemuneration, item.Availability, item.CandidateDescription, item.IdCandidate];
    row += '<td><input type="checkbox" value="'+array+'"></td>';
    
    row += '</tr>';

    return row;
}

function ChangeCandidateCLS(idcandidate) {
    var t = $("#cls" + idcandidate + " option:selected").val() >= 0;
    if ($("#cls" + idcandidate + " option:selected").val() >= 0) {
        $.post("/Adn/ChangeCandidatesCandidateLeadStatus", { idlead: $("#IdLeadSafe").val(), idcandidate: idcandidate, idcls: $("#cls" + idcandidate + " option:selected").val() }).done(
            function (result) {
                if (result == "True") { alert("Changes have been commited."); }
                else {alert("An erro has occured.");
            }
            }
        );
    }
}

function GetCandidateLeadStatus() {

    var cls = {
        IdLeadStatus: 0,
        Description: "",
        Active: 0,
    };
    var list;
    $.ajax({
        type: 'GET',
        url: "/Adn/JSONGetCandidateLeadStatus",
        async: false,
        data: JSON.stringify(cls),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (e) {
            alert("ERROR -> " + e.responseText);
            list = "";
        },
        success: function (result) {
            list = result;
        }
    });

    return list;
}

function getCandidatesInfoForMail() {
   
    var box = [];
    var container = document.getElementById("CandidateResults"); //$("#CandidateResults");
    var results = $('input[type=checkbox]');
    $.each(results, function (index, item) {
        var k = item.value;

        var array = Array.from(k);
        if (item.checked == true) {
            box.push(item.value);
            //alert(item.value);
        }
    });
    var LeadCode;
    $.each(lista, function (index, item) {
        if (item.IdLead == $('#IdLeadSafe').val())
            LeadCode = item.ClientName + " - "+ item.LeadCode;
    });

    $.ajax({
        type: 'POST',
        url: '/Adn/SendCandidatesLead',
        data: { leadCode: LeadCode, box: box, idLead: $("#IdLeadSafe").val() },
        success: function (result) {
        },
        failed: function (result) { alert(result); },
    });

} 

function CreateLead() {

    var Candidates = {
        IdLead: 0,
        IdCustomer: 0,
        IdLeadStatus: 0,
        LeadCode: "",
        Description: "",
        IdSkill: 0,
        SkillExp: 0,
        IdRole: 0,
        RoleExp: 0,
        MinProfExp: 0,
        MaxProfExp: 0,
        MinRemuneartion: 0,
        MaxRemuneartion: 0,
        MinAge: 0,
        MaxAge: 0,
        MaxAvailability: 0,
        ClientName: "",
        NIF: 0,
        LegalRepresentative: "",
        Adress: "",
        Email: "",
        Financial_ProcurementDepartmentEmail: "",
        IdAvailability: 0,
        AvailabilityDescription: "",
        IdNationality: 0,
        NationalityDescription: "",
        LeadStatusDescription: "",
        LeadStatusColor: "",
        NameSkill: "",
        NameRole: "",
    };
}
////////////////////////////////////////////////////////////////////////////
/////////////////////////////Lead Status////////////////////////////////////
function showLeadStatus() {
    var list = $.getJSON("/Adn/JSONLeadStatusList", showLeadStatusList);
}

function showLeadStatusList(list) {
    var container = document.getElementById("List");
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>Color</th><th>Color<br/>Code</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        if (item.Color != null) {
            var r = item.Color.charAt(0) + item.Color.charAt(1);
            var g = item.Color.charAt(2) + item.Color.charAt(3);
            var b = item.Color.charAt(4) + item.Color.charAt(5);
        }

        table += "<tr><td>";

        table += "<button  onclick='FillEditFormCandidateStatus(" + item.IdLeadStatus + ",\"" + item.Description + "\",\"" + r + "\",\"" + g + "\",\"" + b + "\"," + item.Active +
            ")'>Edit</button>";

        table += "</td><td> " + item.Description + "</td><td style='background-color:#" + item.Color + "'>    </td><td>#" + item.Color + "</td>"

        if (item.Active == 1)
            table += "<td>Yes</td>";
        else
            table += "<td>No</td>";
    });

    table += "</tr></table>";

    container.innerHTML = table;
}

////////////////////////////////////////////////////////////////////////////
/////////////////////////////Nationalities//////////////////////////////////
function showNationalites() {
    var list = $.getJSON("/Adn/JSONNationalitiesList", showNationalitiesList);
}

function showNationalitiesList(list) {
    var container = document.getElementById("List");
    container.innerHTML = "";

    var table = "<table border='1'><tr><th>Action</th><th>Description</th><th>Candidates</th><th>Leads</th><th>Status</th></tr>";

    $.each(list, function (index, item) {

        table += "<tr><td>";

        table += "<button  onclick='fillEditFormNationalities(" + item.IdNationality + ",\"" + item.NationalityDescription + "\"," + item.forCandidates + "," + item.forLeads + "," + item.Active +
            ")'>Edit</button>";

        table += "</td><td> " + item.NationalityDescription + "</td>";

        if (item.forCandidates == 1)
            table += "<td>Yes</td>";
        else
            table += "<td>No</td>";

        if (item.forLeads == 1)
            table += "<td>Yes</td>";
        else
            table += "<td>No</td>";

        if (item.Active == 1)
            table += "<td>Yes</td>";
        else
            table += "<td>No</td>";
    });

    table += "</tr></table>";

    container.innerHTML = table;
}

//////////////////////////////////////////////////////////////////////////////
///////////////////////Configurations/////////////////////////////////////////

function GetLogPath() {
    try {
        var Config = {
            LoggingFilesPath:"",
        };

        $.ajax({
            type: 'GET',
            url: "/Adn/GetLogPath",
            async: false,
            data: JSON.stringify(Config),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (e) {
                //alert("ERROR -> " + e.responseText);
                return "Failed to get Logging Path";
            },
            success: function (result) {
                document.getElementById("LogPath").innerText = result;
            }
        });
    } catch (err) {
        return "<p>A problem has appeared. Please inform the developer of this occurrence.</p><br/><p> Have a nice day :)</p>";
    }    
}

function UploadFile() {
    //*var r = $("#FilePath").val();
    if ($("#FilePath").val() != "") {
        $.ajax({
            type: 'POST',
            url: "/Adn/ReadFileAction",
            data: { path: $("#FilePath").val() },
            error: function (e) {
                return "Failed to get File path!";
            },
            success: function (result) {
                if (result == "True") { alert("The file's data has been commited to the DB."); }
                else { alert("Failed to commit to the DB."); }
                
            }
        });
    }
    else
        alert("No path has been given.");
    
}

//////////////////////////////////////////////////////////////////////////////
///////////////////////LogIn/////////////////////////////////////////////////
function LogIn(){
    try {
        $.ajax({
            type: 'POST',
            url: '/Adn/LogInAction',
            data: { user: $("#user").val(), password: $("#password").val() },
            success: function (result) {
                //$("#EditLeadNoteForm").hide();
                //if (result == "True") {
                //    GetLeadNotes();
                //    ListLeadNotes($("#IdLeadSafe").val());
                //} else alert("Aconteceu um erro. A nota não foi apagada.");
                //alert("ola");
                var i = 0;
                if (result != -1) {
                    $("#FailMessage").show();
                }
                else {
                    window.location.href = "/Adn/Index";
                }
             },
            failed: function (result) { alert(result); },
        });
    } catch (err) {
        return "<p>A problem has appeared. Please inform the developer of this occurrence.</p><br/><p> Have a nice day :)</p>";
    }    
}



//////////////////////////////////////////////////////////////////////////////
///////////////////////TESTE/////////////////////////////////////////////////
function SubmitCreateNationality() {
    $('#createNation').on('submit', function (e) {
        e, preventDefault();
        var details = $('#createNation').serialize();
        $.post('Nationalities.cshtml', details, function (data) {
            $('#createNation').html(data);
        });
    });
}

////////////////////////////////////////////////////////////////////

/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */
function ShowMenus(menu) {

    if (menu == 1) {
        //document.getElementById("CandidatesMenu").classList.toggle("show");
        //document.getElementById("LeadsMenu").classList.toggle("hide");
        document.getElementById("CandidatesMenu").style.display = "block";
        document.getElementById("LeadsMenu").style.display = "none";
    }
            
    if (menu == 2) {
        document.getElementById("CandidatesMenu").style.display = "none";
        document.getElementById("LeadsMenu").style.display = "block";
    }
    

}

// Close the dropdown if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            //if (openDropdown.classList.contains('hide')) {
            if (openDropdown.style.display == "block") {
                 //openDropdown.classList.remove('show');
                //openDropdown.classList.toggle('hide');
                openDropdown.style.display = "none";
            }
        }
    }
}