const doc = document,
    $rendBody = doc.getElementById("renderBody");

const containerUser = doc.getElementById("renderLocalUser");
var pageReturnData = {
    container: "renderLocalUser",
    controllerMethod: "/Privileges/User/_ShowPrivileges",
};


var formElement = document.getElementById("form_edit_user");
var originalFormValues = {};


$rendBody.addEventListener("click", (e) => {
    let page = e.target.dataset.value || "";
    //let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
    //let objPaginacion = {
    //    palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
    //    NumPagina: numPage,
    //    CantRegistros: doc.getElementById("tamanoPagina").value,
    //    totalPaginas: doc.getElementById("totalPag").textContent,


    //    urlPagina: "Privileges/User/_TableUser",
    //    containerPag: "userTable"

    //};
    //localStorage.setItem("objPaginacion", JSON.stringify(objPaginacion));


    //===================================================
    //=============  BOTON VISTA AGREGAR  ===============
    //================   USUARIO   ======================
    //===================================================

    if (e.target.id === "create_User") {

        const form_user = doc.getElementById("form_add_user");
        doc.getElementById('btn_modal_user').name = 'btn_modal_save_user';



        doc.getElementById('form_add_user').reset();


        //Add attribute de attribute 
        doc.getElementById('contraseña').setAttribute("required", "");


        doc.getElementById("modaltitleUsuario").textContent = "Agregar Usuario";
        doc.getElementById("btn_modal_user").value = "Agregar";
        $("#add_user_modal").modal('show');
        return false;
    }
    //===================================================
    //=============   BOTON EDITAR   ====================
    //================   USUARIO   ======================
    //===================================================
    else if (e.target.id === "btn_edit_user") {


        doc.getElementById('btn_modal_user').name = 'btn_modal_edit_user';




        fetchMethod({
            url: "/Privileges/User/GetUserData?UserName=" + e.target.dataset.id,
            cbSuccess: (res) => {
                if (res.user != null) {

                    const { nombreUsuario, nombre, identification, correoElectronico,
                        telefono, tipo, whatsApp, activo } = res.user;



                    doc.getElementById('edit_nombreUsuario').value = nombreUsuario;
                    doc.getElementById('edit_nombre').value = nombre;
                    doc.getElementById('edit_cedula').value = identification;
                    doc.getElementById('edit_email').value = correoElectronico;
                    doc.getElementById('edit_rol').value = tipo;
                    doc.getElementById('edit_activo').checked = activo;
                    doc.getElementById('edit_tel').value = telefono;
                    doc.getElementById('edit_whatsapp').value = whatsApp;

                    //Remove de attribute 
                    doc.getElementById('contraseña').removeAttribute('required');

                    originalValues = getFormValues(formElement);


                }
            }

        });

        //Set data to form


        $("#edit_user_modal").modal('show');


        return false;
    }

    //====================================================
    //=============   SHOW INFO   ========================
    //================   USER   ==========================
    //====================================================
    else if (e.target.id === "more_info") {

        fetchMethod({
            url: "/Privileges/User/GetUserData?UserName=" + e.target.dataset.id,
            cbSuccess: (res) => {

                if (res.user != null) {

                    const { nombreUsuario, nombre, identification, correoElectronico,
                        telefono, tipo, whatsApp, activo } = res.user;


                    doc.getElementById('info_nameUser').textContent = nombreUsuario;
                    doc.getElementById('info_name').textContent = nombre;
                    doc.getElementById('info_cedula').textContent = identification;
                    doc.getElementById('info_email').textContent = correoElectronico;

                    doc.getElementById('info_tel').textContent = telefono;
                    doc.getElementById('info_whatsApp').textContent = whatsApp;

                    //Remove de attribute 

                    $("#user_information_modal").modal('show');
                }
            }

        });

        return false;
    }

    //====================================================
    //=============   DELETE THE   =======================
    //================   USER   ==========================
    //====================================================
    else if (e.target.id === "delete_user") {


        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success  ',
                cancelButton: 'btn btn-danger buttonModal'
            },
            buttonsStyling: false
        })

        swalWithBootstrapButtons.fire({
            title: 'Desea eliminar el Usuario?',
            text: "No se podrá revertir esta acción!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí,Eliminar!',
            cancelButtonText: 'Cancelar!',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {

                fetchMethod({
                    url: "/Privileges/User/DeleteUser?UserName=" + e.target.dataset.id,
                    cbSuccess: (res) => {

                        console.log(res);
                        if (res != null) {
                            if (res.result[0] == 1) {

                                Swal.fire(
                                    'Elimiado!',
                                    res.result[1],
                                    'success',
                                    4000,

                                );

                                let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                                cargarComponent({
                                    container: "userTable",
                                    url: "/Privileges/User/_TableUsers",
                                    body: {
                                        palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
                                        NumPagina: numPage,
                                        CantRegistros: doc.getElementById("tamanoPagina").value,
                                        totalPaginas: doc.getElementById("totalPag").textContent,
                                    }

                                });
                            }
                            else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: res.result[1],
                                    timer: 4000,

                                });
                            }
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Ocurrió un error al eliminar el usuario!!',
                                timer: 4000,

                            });

                        }
                    }

                });

            } else if (

                result.dismiss === Swal.DismissReason.cancel
            ) {
                swalWithBootstrapButtons.fire(
                    'Cancelado',
                    'Acción Cancelada :)',
                    'error',
                    4000,
                )
            }
        })


        //

        return false;
    }

    //===================================================
    //=============   BOTON ANT   =======================
    //================   PAGE   =========================
    //===================================================
    else if (e.target.id === "previousPage") {

        retrocederPagina({
            ant: page,
            totalPage: doc.getElementById("totalPag").textContent,
            tamanoPagina: doc.getElementById("tamanoPagina").value,
            url: "/Privileges/User/_TableUsers",
            container: "userTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()
        })
    }
    //===================================================
    //=============   BOTON NEXT   =======================
    //================   PAGE   ==========================
    //====================================================
    else if (e.target.id === "nextPage") {
        siguientePagina({
            sig: page,
            totalPage: doc.getElementById("totalPag").textContent,
            CantRegistros: doc.getElementById("tamanoPagina").value,
            url: "/Privileges/User/_TableUsers",
            container: "userTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()
        });

    }

    //====================================================
    //=============   ADD THE USER   =====================
    //================   PROFILE  ========================
    //====================================================
    else if (e.target.id === "addPerfilUsuario") {

        var Usuario = doc.getElementById('edit_nombreUsuario').value;

        var idPerfil = doc.getElementById('perfilSelect').value;

        console.log(idPerfil);
        if (idPerfil > 0) {
            fetchMethod({
                url: "/Privileges/Profile/agregarUsuarioPerfil",
                body: {

                    Usuario: doc.getElementById('edit_nombreUsuario').value,


                    IdPerfil: doc.getElementById('perfilSelect').value,

                },
                cbSuccess: (res) => {

                    if (res.result && res != null) {



                        let numPage = Number(doc.getElementById("pagActualPU").textContent) - 1;
                        cargarComponent({
                            container: "tableProfilesUser",
                            url: "/Privileges/User/_ProfilesUser",
                            body: {
                                Usuario: doc.getElementById('edit_nombreUsuario').value,
                                palabraBuscar: doc.getElementById("buscarPU").value.toLowerCase(),
                                NumPagina: numPage,
                                CantRegistros: doc.getElementById("tamanoPaginaPU").value,
                                totalPaginas: doc.getElementById("totalPagPU").textContent,
                            }

                        });
                        cargarComponent({
                            container: "selectProfiles",
                            url: "/Privileges/User/_SelectOpcProfile",
                            body: {
                                NombreUsuario: doc.getElementById('edit_nombreUsuario').value

                            }

                        });


                        Swal.fire({

                            icon: 'success',
                            title: 'Bien!',
                            text: 'Agregado Correctamente.',
                            timer: 4000,
                            customClass: {
                                container: 'rounded-modal' // Aquí establecemos la clase personalizada para el modal
                            }
                        });
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'No se asocio el usuario al perfil...',
                            timer: 4000,

                        })
                    }
                }


            });

        }

        return false;
    }

    //====================================================
    //=============   DELETE THE USER   =====================
    //================   PROFILE  ========================
    //====================================================
    else if (e.target.id === "eliminarPerfilUsuario") {
        var title = "¿Eliminar el Perfil del Usuario?";
        var text = '';


        var Usuario = doc.getElementById('edit_nombreUsuario').value;

        var idPerfil = e.target.dataset.id;
        console.log("Usuario: " + Usuario + " perfil: " + idPerfil);
        alertDelete(title, text)
            .then((confirmed) => {
                if (confirmed) {

                    fetchMethod({
                        url: "/Privileges/Profile/eliminarUsuarioPerfil",
                        body: {
                            Usuario: doc.getElementById('edit_nombreUsuario').value,


                            IdPerfil: e.target.dataset.id,
                        },
                        cbSuccess: (res) => {

                            if (res != null) {
                                if (res.result) {

                                    let numPage = Number(doc.getElementById("pagActualPU").textContent) - 1;
                                    cargarComponent({
                                        container: "tableProfilesUser",
                                        url: "/Privileges/User/_ProfilesUser",
                                        body: {
                                            Usuario: doc.getElementById('edit_nombreUsuario').value,
                                            palabraBuscar: doc.getElementById("buscarPU").value.toLowerCase(),
                                            NumPagina: numPage,
                                            CantRegistros: doc.getElementById("tamanoPaginaPU").value,
                                            totalPaginas: doc.getElementById("totalPagPU").textContent,
                                        }

                                    });
                                    cargarComponent({
                                        container: "selectProfiles",
                                        url: "/Privileges/User/_SelectOpcProfile",
                                        body: {
                                            NombreUsuario: doc.getElementById('edit_nombreUsuario').value

                                        }

                                    });


                                    Swal.fire(
                                        'Elimiado!',
                                        'Se removió el Usuario',
                                        'success',
                                        4000,

                                    );

                                }
                                else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Oops...',
                                        text: 'Ocurrió un error!!',
                                        timer: 4000,

                                    });
                                }
                            }
                            else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: 'Ocurrió un error al eliminar el perfil!!',
                                    timer: 4000,

                                });

                            }
                        }

                    });
               
                }

            });

        return false;
    }
    //===================================================
    //=============   BOTON NEXT   =======================
    //================   PAGE   ==========================
    //================ PROFILE USER ======================
    //====================================================
    else if (e.target.id === "nextPageUP") {

        siguientePagina({
            idRelacion: doc.getElementById("edit_nombreUsuario").value,
            sig: page,
            totalPage: doc.getElementById("totalPagPU").textContent,
            CantRegistros: doc.getElementById("tamanoPaginaPU").value,
            url: "/Privileges/User/_ProfilesUser",            
            container: "tableProfilesUser",
            
            palabraBuscar: doc.getElementById("buscarPU").value.toLowerCase()
        });
        return false;
    }
    //===================================================
    //=============   BOTON ANT   =======================
    //================   PAGE   =========================
    //============= USER PAROFILE   =====================
    //===================================================
    else if (e.target.id === "previousPageUP") {

        retrocederPagina({
            idRelacion: doc.getElementById("edit_nombreUsuario").value,
            ant: page,
            totalPage: doc.getElementById("totalPagPU").textContent,
            tamanoPagina: doc.getElementById("tamanoPaginaPU").value,
            url: "/Privileges/User/_ProfilesUser",
            container: "tableProfilesUser",

            palabraBuscar: doc.getElementById("buscarPU").value.toLowerCase()
        })
        return false;
    }

});







$rendBody.addEventListener("submit", (e) => {
    e.preventDefault();


    if (e.target.id === "form_edit_user") {
        const btnModal = doc.getElementById('btn_modal_user');
        //===================================================
        //=============  BOTON Editar   ====================
        //================   USUARIO   ======================
        //===================================================


        //Get data of the form to verify if it changes
        var currentFormValues = getFormValues(formElement);

        console.log(compareFormValues(originalFormValues, currentFormValues));
        //verify data 
        if (compareFormValues(originalFormValues, currentFormValues)) {



            fetchMethod({
                url: "/Privileges/User/UpdateUser",
                body: {

                    NombreUsuario: doc.getElementById('edit_nombreUsuario').value,
                    Contrasena: doc.getElementById('edit_contraseña').value,
                    Nombre: doc.getElementById('edit_nombre').value,
                    Identification: doc.getElementById('edit_cedula').value,
                    CorreoElectronico: doc.getElementById('edit_email').value,
                    Tipo: doc.getElementById('edit_rol').value,
                    Activo: doc.getElementById('edit_activo').checked,
                    Telefono: doc.getElementById('edit_tel').value,
                    whatsApp: doc.getElementById('edit_whatsapp').value,




                },
                cbSuccess: (res) => {
                    console.log(res);       console.log(res);
                    if (res.result) {
                        //$("#add_user_modal").modal('hide');

                        //let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                        //cargarComponent({
                        //    container: "userTable",
                        //    url: "/Privileges/User/_TableUsers",
                        //    body: {
                        //        palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
                        //        NumPagina: numPage,
                        //        CantRegistros: doc.getElementById("tamanoPagina").value,
                        //        totalPaginas: doc.getElementById("totalPag").textContent,
                        //    }

                        //});

                        Swal.fire({

                            icon: 'success',
                            title: 'Bien!',
                            text: 'Actualizado Correctamente.',
                            timer: 4000,
                            customClass: {
                                container: 'rounded-modal' // Aquí establecemos la clase personalizada para el modal
                            }
                        });
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'No se actualizó el usuario...',
                            timer: 4000,

                        })
                    }
                }


            });

            /*$("#add_user_modal").modal('hide');*/

        }
    }
    //===================================================
    //=============  BOTON GUARDAR   ====================
    //================   USUARIO   ======================
    //===================================================
    else if (e.target.id === "form_add_user") {

        fetchMethod({
            url: "/Privileges/User/CreateUser",
            body: {

                NombreUsuario: doc.getElementById('nombreUsuario').value,
                Contrasena: doc.getElementById('contraseña').value,
                Nombre: doc.getElementById('nombre').value,
                Identification: doc.getElementById('cedula').value,
                CorreoElectronico: doc.getElementById('email').value,
                Tipo: doc.getElementById('rol').value,
                Activo: doc.getElementById('activo').checked,
                Telefono: doc.getElementById('tel').value,
                whatsApp: doc.getElementById('whatsapp').value,

            },
            cbSuccess: (res) => {

                if (res.result && res != null) {
                    $("#add_user_modal").modal('hide');

                    let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                    cargarComponent({
                        container: "userTable",
                        url: "/Privileges/User/_TableUsers",
                        body: {
                            palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
                            NumPagina: numPage,
                            CantRegistros: doc.getElementById("tamanoPagina").value,
                            totalPaginas: doc.getElementById("totalPag").textContent,
                        }

                    });
                    Swal.fire({

                        icon: 'success',
                        title: 'Bien!',
                        text: 'Ingresado Correctamente.',
                        timer: 4000,
                        customClass: {
                            container: 'rounded-modal' // Aquí establecemos la clase personalizada para el modal
                        }
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'No se agrego el usuario...',
                        timer: 4000,

                    })
                }
            }


        });
    }



    return false;

});


$rendBody.addEventListener("change", (e) => {
    if (e.target.name === "state_user") {
        const selectItem = doc.getElementById('state_user');


        fetchMethod({
            url: "/Privileges/User/ChangeStatus?userName=" + e.target.dataset.id,
            cbSuccess: (res) => {

                if (res.result) {
                    Swal.fire({
                        /*position: 'top-end',*/
                        icon: 'success',
                        title: 'Bien!',
                        text: 'Se modificó el estado correctamente!',
                        timer: 4000,
                        customClass: {
                            container: 'rounded-modal' // Aquí establecemos la clase personalizada para el modal
                        }
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'No se modificó el estado...',
                        timer: 4000,

                    })

                }

            }


        });

        return false;
    }
    else if (e.target.id === "tamanoPagina") {

        cambioTamanoPagina({
            totalPage: doc.getElementById("totalPag").textContent,
            tamanoPagina: e.target.value,
            url: "/Privileges/User/_TableUsers",
            container: "userTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()
        });

        return false;
    }


    $("#buscar").on("change", function () {

        let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
        let objPaginacion = {
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
            NumPagina: numPage,
            CantRegistros: doc.getElementById("tamanoPagina").value,
            totalPaginas: doc.getElementById("totalPag").textContent,

            urlPagina: "/Privileges/User/_TableUser",
            containerPag: "userTable"

        }

        cargarComponent({
            container: "userTable",
            url: "/Privileges/User/_TableUsers",
            body: objPaginacion
        })

    });
});



//===============================================
//=============  FUNCIONES   ====================
//===============================================




