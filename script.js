/*Assignment 2 UniBook
Abdulmajeed Almeri: 2338695 & Riyadh Barnawi: 2336571
*/
function validateLogin(){
    var username = document.forms["loginForm"]["Username"].value;
    var password = document.forms["loginForm"]["psword"].value;
    if(username == ""){
        document.getElementById("UserMsg").innerHTML="Username is required";
        document.getElementById("UserMsg").style.color="red";
    }else{
        document.getElementById("UserMsg").innerHTML = "";
    }
    if(password == ""){
        document.getElementById("PassMsg").innerHTML="Password is required";
        document.getElementById("PassMsg").style.color="red";
    }else{
        document.getElementById("PassMsg").innerHTML = "";
    }
    if(username != "" && password != ""){
        document.getElementById("loginResult").innerHTML = "Login successful, welcome to UniBook";
        document.getElementById("loginResult").style.color = "green";
    }else{
        document.getElementById("loginResult").innerHTML = "";
    }
return false;
}
function validateSignup(){
    var Fname = document.forms["signupForm"]["fname"].value;
    var Lname = document.forms["signupForm"]["lname"].value;
    var username = document.forms["signupForm"]["Username"].value;
    var email = document.forms["signupForm"]["email"].value;
    var password = document.forms["signupForm"]["psw"].value;
    var Cpassword = document.forms["signupForm"]["cpsw"].value;
    var fac = document.forms["signupForm"]["fac"];
    var facChosen = false;


    if(Fname == ""){
        document.getElementById("FnameMsg").innerHTML="First name is required";
        document.getElementById("FnameMsg").style.color="red";
    }else{
        document.getElementById("FnameMsg").innerHTML = "";
    }
    if(Lname == ""){
        document.getElementById("LnameMsg").innerHTML="Last name is required";
        document.getElementById("LnameMsg").style.color="red";
    }else{
        document.getElementById("LnameMsg").innerHTML = "";
    }
    if(username == ""){
        document.getElementById("UserMsg").innerHTML="Username is required";
        document.getElementById("UserMsg").style.color="red";
    }else{
        document.getElementById("UserMsg").innerHTML = "";
    }
    if(email == ""){
        document.getElementById("EmailMsg").innerHTML="Email is required";
        document.getElementById("EmailMsg").style.color="red";
    }else{
        document.getElementById("EmailMsg").innerHTML = "";
    }
    if(password == ""){
        document.getElementById("PswMsg").innerHTML="Password is required";
        document.getElementById("PswMsg").style.color="red";
    }else{
        document.getElementById("PswMsg").innerHTML = "";
    }
    if(Cpassword == ""){
        document.getElementById("CpswMsg").innerHTML="Confirm Password";
        document.getElementById("CpswMsg").style.color="red";
    }else if (password != Cpassword){
        document.getElementById("CpswMsg").innerHTML="Password does not match";
        document.getElementById("CpswMsg").style.color="red";
    }else{
        document.getElementById("CpswMsg").innerHTML = "";
    }
    if(Fname != "" && Lname != "" && username != "" && email != "" && password != "" && Cpassword!= "" && password == Cpassword){
        document.getElementById("signupResult").innerHTML = "Account created successfully";
        document.getElementById("signupResult").style.color = "green";
    }
        else{
        document.getElementById("signupResult").innerHTML = "";
    }
return false;
}
function clearupSignup() {
    document.getElementById("FnameMsg").innerHTML = "";
    document.getElementById("LnameMsg").innerHTML = "";
    document.getElementById("UserMsg").innerHTML = "";
    document.getElementById("EmailMsg").innerHTML = "";
    document.getElementById("PswMsg").innerHTML = "";
    document.getElementById("CpswMsg").innerHTML = "";
    document.getElementById("FacMsg").innerHTML = "";
    document.getElementById("signupResult").innerHTML = "";
}
function showPrices() {
    var text = "";
    for ( var i = 1; i <= 5; i++ ){
        text = text + i + " copies = " + (i * 160) + " SR<br>";
    }
    document.getElementById("priceList").innerHTML = text;
}
function bookOver() {
    document.getElementById("book1").style.border = "3px solid green";
    document.getElementById("bookCaption").innerHTML = "CPIS-358 book: 160 SR new, 95 SR used";
}

function bookOut() {
    document.getElementById("book1").style.border = "none";
    document.getElementById("bookCaption").innerHTML = "";
}
function showSearch() {
    var query = document.getElementById("searchBox").value;
    if (query == "") {
        document.getElementById("searchMsg").style.color = "red";
        document.getElementById("searchMsg").innerHTML = "Please type a book name";
    } else {
        document.getElementById("searchMsg").style.color = "green";
        document.getElementById("searchMsg").innerHTML = "Searching for: " + query;
    }
}
function toggleTeam() {
    if (document.getElementById("teamSection").style.display == "none") {
        document.getElementById("teamSection").style.display = "block";
    } else {
        document.getElementById("teamSection").style.display = "none";
    }
}

