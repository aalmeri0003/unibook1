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
return false
}
