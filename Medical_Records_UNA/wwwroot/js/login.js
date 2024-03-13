//const d = document,
//    $renderBody = d.getElementById("renderBody");


//===========================================
//================ VARIABLES ================
//===========================================

const email = document.getElementById("username");
const password = document.getElementById("password");

//Boton Inicio Sesion
const btnAccess = document.getElementById("btnAccess");

eventsListener();
function eventsListener() {
    btnAccess.addEventListener("click", (e) => {
        loginAccess(e);
    });
}

//===========================================
//============= FUNCIONES ===================
//===========================================


function verifyInputs() {

    if ((email.value == undefined || email.value == null || email.value == "")
        ||
        (password.value == undefined || password.value == null || password.value == "")) {
        
        failedAlert();
        return false;
    }
    return true;

}

function loginAccess(e) {

    e.preventDefault();

    
    if (verifyInputs()) {
        
        Swal.fire({
            icon: null,
            title: 'Espere.....',
            text: 'Cargando',
            allowEscapeKey: false,
            allowOutsideClick: false,
            showConfirmButton: false,
            /*timer:2000,*/
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading()

            }
        })
        var data = {
            NombreUsuario: email.value,
            Contrasena: password.value
        };
        fetch("/Access/AccessVerify", {

            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(res => res.ok ? res.json() : null)
            .then(res => {
                    
                    if (res.result === "1") {
                        successAlert();
                        setTimeout(function () {
                            window.location.href = '/Home/Index';
                        }, 2000);


                        

                    } else {
                        failedAlert();
                    }

                }
            );       
    }
}

function successAlert() {
    Swal.fire({
        /*position: 'top-end',*/
        icon: 'info',
        title: 'Ingreso Correcto',
        showConfirmButton: false,
        timer: 4000
    })

}
function failedAlert() {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Usuario o Contraseña Incorrecta',
        timer: 4000,

    })
}