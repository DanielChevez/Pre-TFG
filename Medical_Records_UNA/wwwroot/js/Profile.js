const doc = document,
    $rendBody = doc.getElementById("renderBody");
var formElement = document.getElementById("form_edit_profile");
var originalFormValues = {};



$rendBody.addEventListener("click", (e) => {
    let page = e.target.dataset.value || "";

    //===================================================
    //=============  BOTON VISTA AGREGAR  ===============
    //================    PERFIL   ======================
    //===================================================

    if (e.target.id === "create_Profile") {

        doc.getElementById('btn_modal_profile').name = 'btn_modal_save_profile';



        doc.getElementById('form_add_profile').reset();



        doc.getElementById("modaltitlePerfil").textContent = "Agregar Perfil";
        doc.getElementById("btn_modal_profile").value = "Agregar";
        $("#add_profile_modal").modal('show');

        return false;
    }
    //====================================================
    //=============   ADD THE USER   =====================
    //================   PROFILE  ========================
    //====================================================
    else if (e.target.id === "btnAgregarUsuarioPerfil") {

        var Usuario = doc.getElementById('userSelect').value;

        if (Usuario > 0 || Usuario != null || Usuario != "") {
            fetchMethod({
                url: "/Privileges/Profile/agregarUsuarioPerfil",
                body: {

                    Usuario: doc.getElementById('userSelect').value,


                    IdPerfil: doc.getElementById('id_perfil_edit').value,

                },
                cbSuccess: (res) => {

                    if (res.result && res != null) {



                        let numPage = Number(doc.getElementById("pagActualUP").textContent) - 1;


                        cargarComponent({
                            container: "tableUserProfile",
                            url: "/Privileges/Profile/_UsersProfile",
                            body: {
                                Usuario: doc.getElementById("id_perfil_edit").value,
                                palabraBuscar: doc.getElementById("buscarUP").value.toLowerCase(),
                                NumPagina: numPage,
                                CantRegistros: doc.getElementById("tamanoPaginaUP").value,
                                totalPaginas: doc.getElementById("totalPagUP").textContent,
                            }

                        });
                        cargarComponent({
                            container: "selectUsersProfile",
                            url: "/Privileges/Profile/_SelectOpcUser",
                            body: {
                                IdPerfil: doc.getElementById("id_perfil_edit").value

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
    else if (e.target.id === "eliminarUsuarioPerfil") {
        var title = "¿Eliminar el Usuario del Perfil?";
        var text = '';

        alertDelete(title, text)
            .then((confirmed) => {
                if (confirmed) {

                    fetchMethod({
                        url: "/Privileges/Profile/eliminarUsuarioPerfil",
                        body: {
                            Usuario: e.target.dataset.id,


                            IdPerfil: doc.getElementById('id_perfil_edit').value,
                        },
                        cbSuccess: (res) => {

                            if (res != null) {
                                if (res.result) {

                                    Swal.fire(
                                        'Elimiado!',
                                        'Se removió el Usuario',
                                        'success',
                                        4000,

                                    );
                                    let numPage = Number(doc.getElementById("pagActualUP").textContent) - 1;
                                    cargarComponent({
                                        container: "tableUserProfile",
                                        url: "/Privileges/Profile/_UsersProfile",
                                        body: {
                                            Usuario: doc.getElementById("id_perfil_edit").value,
                                            palabraBuscar: doc.getElementById("buscarUP").value.toLowerCase(),
                                            NumPagina: numPage,
                                            CantRegistros: doc.getElementById("tamanoPaginaUP").value,
                                            totalPaginas: doc.getElementById("totalPagUP").textContent,
                                        }

                                    });


                                    cargarComponent({
                                        container: "selectUsersProfile",
                                        url: "/Privileges/Profile/_SelectOpcUser",
                                        body: {
                                            IdPerfil: doc.getElementById("id_perfil_edit").value
                                        }
                                    });

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
    //====================================================
    //=============   DELETE THE   =======================
    //===============   PROFILE   ========================
    //====================================================
    else if (e.target.id === "delete_profile") {
        var title = "¿Estás seguro que quieres eliminar este perfil?";
        var text = 'No se podrá revertir esta acción!';


        alertDelete(title, text)
            .then((confirmed) => {
                if (confirmed) {
                    fetchMethod({
                        url: "/Privileges/Profile/DeleteProfile?id=" + e.target.dataset.id,
                        cbSuccess: (res) => {


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
                                        container: "profileTable",
                                        url: "/Privileges/Profile/_TableProfiles",
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
    //=============   BOTON ANT   =======================
    //================   PAGE   =========================
    //===================================================
    else if (e.target.id === "previousPage") {

        retrocederPagina({
            ant: page,
            totalPage: doc.getElementById("totalPag").textContent,
            tamanoPagina: doc.getElementById("tamanoPagina").value,
            url: "/Privileges/Profile/_TableProfiles",
            container: "profileTable",

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
            url: "/Privileges/Profile/_TableProfiles",
            container: "profileTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()




        });

    }

    //===================================================
    //=============   BOTON NEXT   =======================
    //================   PAGE   ==========================
    //================ USER PROFILE  ======================
    //====================================================
    else if (e.target.id === "nextPageUP") {

        siguientePagina({
            idRelacion: doc.getElementById("id_perfil_edit").value,
            sig: page,
            totalPage: doc.getElementById("totalPagUP").textContent,
            CantRegistros: doc.getElementById("tamanoPaginaUP").value,
            url: "/Privileges/Profile/_UsersProfile",
            container: "tableUserProfile",
            palabraBuscar: doc.getElementById("buscarUP").value.toLowerCase()
        });

    }
    //===================================================
    //=============   BOTON ANT   =======================
    //================   PAGE   =========================
    //============= USER PAROFILE   =====================
    //===================================================
    else if (e.target.id === "previousPageUP") {

        retrocederPagina({
            idRelacion: doc.getElementById("id_perfil_edit").value,
            ant: page,
            totalPage: doc.getElementById("totalPagUP").textContent,
            tamanoPagina: doc.getElementById("tamanoPaginaUP").value,
            url: "/Privileges/Profile/_UsersProfile",
            container: "tableUserProfile",

            palabraBuscar: doc.getElementById("buscarUP").value.toLowerCase()
        })
    }
    //===================================================
    //=================  BOTON  =========================
    //================   SAVE   =========================
    //============= ACTIONS OF PROFILE   ================
    //===================================================
    else if (e.target.id === "btn_save_actions") {
        $("#modalVerifyPassword").modal('show');

    }
});

$rendBody.addEventListener("submit", (e) => {
    e.preventDefault();
    const btnModal = doc.getElementById('btn_modal_profile');

    //===================================================
    //=============  BOTON GUARDAR  ===================
    //================   PERFIL   ======================
    //===================================================

    if (e.target.id === "form_add_profile") {


        fetchMethod({
            url: "/Privileges/Profile/CreateProfile",
            body: {

                Nombre: doc.getElementById('nombre_perfil').value,


                Descripcion: doc.getElementById('descripcion_perfil').value,

                Activo: doc.getElementById('activar_perfil').checked,
            },
            cbSuccess: (res) => {


                if (res.result && res != null) {

                    $("#add_profile_modal").modal('hide');

                    let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                    cargarComponent({
                        container: "profileTable",
                        url: "/Privileges/Profile/_TableProfiles",
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

        return false;
    }
    //===================================================
    //=============  BOTON EDITAR   ====================
    //================   PERFIL   ======================
    //===================================================

    else if (e.target.id === "form_edit_profile") {

        var currentFormValues = getFormValues(formElement);


        //verify data 

        if (compareFormValues(originalFormValues, currentFormValues)) {


            fetchMethod({
                url: "/Privileges/Profile/UpdateProfile",
                body: {
                    IdPerfil: doc.getElementById('id_perfil_edit').value,
                    Nombre: doc.getElementById('edit_nombre_perfil').value,


                    Descripcion: doc.getElementById('edit_descripcion_perfil').value,

                    Activo: doc.getElementById('edit_activar_perfil').checked,
                },
                cbSuccess: (res) => {

                    if (res.result && res != null) {

                        $("#edit_profile_modal").modal('hide');

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
                            text: 'No se actualizó el perfil...',
                            timer: 4000,

                        })
                    }
                }


            });

        }
        $("#add_user_modal").modal('hide');
        return false;
    }

    else if (e.target.id === "verifyPassword") {
        const inputPass = doc.getElementById("password_verify").value;

        console.log("Entra");
        fetchMethod({
            url: "/Privileges/Profile/VerifyPassword?password=" + inputPass,
            cbSuccess: (res) => {
                $("#modalVerifyPassword").modal('hide');
                console.log(res);
                if (res.result) {
                    saveActions();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Contraseña Incorrecta....',
                        timer: 4000,

                    })
                }
                
            }

        });
    }

});

$rendBody.addEventListener("change", (e) => {
    let page = e.target.dataset.value || "";
    if (e.target.name === "state_profile") {

        console.log(e.target.dataset.id);


        fetchMethod({
            url: "/Privileges/Profile/ChangeStatus?IdPerfil=" + e.target.dataset.id,
            cbSuccess: (res) => {

                if (res.result) {
                    Swal.fire({

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
            url: "/Privileges/Profile/_TableProfiles",
            container: "profileTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()
        });

        return false;
    }

    else if (e.target.id === "tamanoPaginaUP") {

        cambioTamanoPagina({
            idRelacion: doc.getElementById("id_perfil_edit").value,
            totalPage: doc.getElementById("totalPagUP").textContent,
            tamanoPagina: e.target.value,
            url: "/Privileges/Profile/_UsersProfile",
            container: "tableUserProfile",
            palabraBuscar: doc.getElementById("buscarUP").value.toLowerCase()
        });
    }

    $("#buscar").on("keyup", function () {

        let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
        let objPaginacion = {
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
            NumPagina: numPage,
            CantRegistros: doc.getElementById("tamanoPagina").value,
            totalPaginas: doc.getElementById("totalPag").textContent,

            url: "/Privileges/Profile/_TableProfiles",
            container: "profileTable",

        }

        cargarComponent({
            url: "/Privileges/Profile/_TableProfiles",
            container: "profileTable",
            body: objPaginacion
        })

    });

    $("#buscarUP").on("keyup", function () {





        let numPage = Number(doc.getElementById("pagActualUP").textContent) - 1;
        let objPaginacion = {
            palabraBuscar: doc.getElementById("buscarUP").value.toLowerCase(),
            NumPagina: numPage,
            CantRegistros: doc.getElementById("tamanoPaginaUP").value,
            totalPaginas: doc.getElementById("totalPagUP").textContent,

            url: "/Privileges/Profile/_UsersProfile",
            container: "tableUserProfile",

        }

        cargarComponent({
            url: "/Privileges/Profile/_UsersProfile",
            container: "tableUserProfile",
            body: objPaginacion
        });

    });
});

//function verifyPassqword() {

//    const btnModal = doc.getElementById("btnSaveActions");

//    const formPassword = doc.getElementById("verifyPassword");
//    var resultValidate = false;
//    $("#modalVerifyPassword").modal('show');


//    formPassword.addEventListener("submit", (e) => {
//        console.log("Entra");
//        e.preventDefault();
       


//    });
//    return false;
//}

function saveActions() {

    const idPerfil = doc.getElementById("id_perfil_edit").value;
    // Obtener todos los checkboxes marcados dentro del contenedor del árbol de privilegios
    const checkedItems = $('#tree input[type="checkbox"]:checked');

    // Crear un array para almacenar los valores de data-id de los checkboxes marcados
    const checkedIds = checkedItems.map(function () {
        return $(this).data('id');
    }).get();


    fetchMethod({
        url: "/Privileges/Profile/SaveActions",
        body: {
            Actions: checkedIds,


            IdPerfil: doc.getElementById('id_perfil_edit').value,
        },
        cbSuccess: (res) => {
            if (res.result) {

                Swal.fire({

                    icon: 'success',
                    title: 'Bien!',
                    text: 'Guardado Correctamente.',
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
                    text: 'No se guardaron los cambios...',
                    timer: 4000,

                })
            }
        }


    });

}