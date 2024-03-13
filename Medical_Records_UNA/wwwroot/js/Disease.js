const doc = document,
    $rendBody = doc.getElementById("renderBody");
//var formElement = document.getElementById("form_edit_profile");
var originalFormValues = {};



$rendBody.addEventListener("click", (e) => {
    let page = e.target.dataset.value || "";

    //===================================================
    //=============  BOTON VISTA AGREGAR  ===============
    //================    PERFIL   ======================
    //===================================================

    if (e.target.id === "create_disease") {


        doc.getElementById("form_add_disease").reset();
        doc.getElementById('btn_modal_disease').name = 'btn_modal_save_disease';

        doc.getElementById("modaltitleDisease").textContent = "Agregar Enfermedad";
        doc.getElementById("btn_modal_disease").value = "Agregar";
        $("#add_disease_modal").modal('show');

        return false;
    }

    else if (e.target.id === "btn_edit_disease") {

        doc.getElementById("modaltitleDisease").textContent = "Editar Enfermedad";
        doc.getElementById("btn_modal_disease").value = "Editar";

        var idDisease = e.target.dataset.id;

        findData(idDisease);



        $("#add_disease_modal").modal('show');
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
            url: "/MedicalConditions/Disease/_TableDiseases",
            container: "diseaseTable",

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
            url: "/MedicalConditions/Disease/_TableDiseases",
            container: "diseaseTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()



        });

    }
    //====================================================
    //=============   DELETE THE   =======================
    //===============   DISEASE   ========================
    //====================================================
    else if (e.target.id === "delete_disease") {
        var title = "¿Estás seguro que quieres eliminar esta Enfermedad?";
        var text = 'No se podrá revertir esta acción!';
 
        alertDelete(title, text)
            .then((confirmed) => {
                if (confirmed) {


                    fetchMethod({
                        url: "/MedicalConditions/Disease/Delete",
                        body: {
                            'Id_Enfermedad': e.target.dataset.id
                        },
                        cbSuccess: (res) => {


                            if (res != null) {
                                if (res.result[1] == 1) {

                                    Swal.fire(
                                        'Elimiado!',
                                        res.result[0],
                                        'success',
                                        4000,

                                    );

                                    let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                                    cargarComponent({
                                        container: "diseaseTable",
                                        url: "/MedicalConditions/Disease/_TableDiseases",
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
                                        text: res.result[0],
                                        timer: 4000,

                                    });
                                }
                            }
                            else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: 'Ocurrió un error al eliminar la enfermedad!!',
                                    timer: 4000,

                                });

                            }
                        }

                    });

                }
            });
        //    else if (

        //        result.dismiss === Swal.DismissReason.cancel
        //    ) {
        //        swalWithBootstrapButtons.fire(
        //            'Cancelado',
        //            'Acción Cancelada :)',
        //            'error',
        //            4000,
        //        )
        //   }
        //})


        return false;

    }
});

$rendBody.addEventListener("submit", (e) => {

    e.preventDefault();

    if (e.target.id === "form_add_disease") {


        const btnModal = doc.getElementById("btn_modal_disease").name;



        sendData();



        $("#add_disease_modal").modal('hide');
        //if (btnModal === "btn_modal_save_disease") {


        //} else {


        //}
        return false;
    }
});

$rendBody.addEventListener("change", (e) => {
    let page = e.target.dataset.value || "";
    if (e.target.id === "tamanoPagina") {
        
        cambioTamanoPagina({
            totalPage: doc.getElementById("totalPag").textContent,
            tamanoPagina: e.target.value,
            url: "/MedicalConditions/Disease/_TableDiseases",
            container: "diseaseTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()
        });

        return false;
    }

    $("#buscar").on("keyup", function () {





        let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
        let objPaginacion = {
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
            NumPagina: numPage,
            CantRegistros: doc.getElementById("tamanoPagina").value,
            totalPaginas: doc.getElementById("totalPag").textContent,

            url: "/MedicalConditions/Disease/_TableDiseases",
            container: "diseaseTable",

        }

        cargarComponent({

            url: "/MedicalConditions/Disease/_TableDiseases",
            container: "diseaseTable",
            body: objPaginacion
        });

    });

});
function getDataFormDisease() {


    var data = {

        'Nombre': doc.getElementById("nombre").value,
        'Descripcion': doc.getElementById("descripcion_enfermedad").value,
        'Estado': doc.getElementById("estado").value,
        'Recomendacion': doc.getElementById("recomendacion_enfermedad").value,
        'Id_Enfermedad': doc.getElementById("id_disease").value,
        'Tipo': doc.getElementById("tipo").value
    };


    return data;
}

function sendData() {

    var data = getDataFormDisease();

    var id = doc.getElementById("id_disease").value;
    
    fetchMethod({
        url: "/MedicalConditions/Disease/Create_Update_Disease",
        body: data
        ,
        cbSuccess: (res) => {

            if (res.result[1]) {

                Swal.fire({

                    icon: 'success',
                    title: 'Bien!',
                    text: res.result[0],
                    timer: 4000,
                    customClass: {
                        container: 'rounded-modal' // Aquí establecemos la clase personalizada para el modal
                    }
                });


                let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                cargarComponent({
                    container: "diseaseTable",
                    url: "/MedicalConditions/Disease/_TableDiseases",
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
                    text: 'No se guardaron los cambios...',
                    timer: 4000,

                })
            }






        }


    });



}

async function findData(id) {

    await fetchMethod({
        url: "/MedicalConditions/Disease/getDisease",
        body: {
            'Id_Enfermedad': id,
        },

        cbSuccess: (res) => {
 
            if (res.disease !== null) {
                const { id_Enfermedad, nombre, descripcion, tipo, recomendacion, estado } = res.disease;


                doc.getElementById("nombre").value = nombre;
                doc.getElementById("descripcion_enfermedad").value = descripcion;
                doc.getElementById("estado").value = estado;
                doc.getElementById("recomendacion_enfermedad").value = recomendacion;
                doc.getElementById("id_disease").value = id_Enfermedad;
                doc.getElementById("tipo").value = tipo;

                // originalFormValues = getDataFormDisease();

            }

        }


    });



}

function Delete(id) {

    fetchMethod({
        url: "/MedicalConditions/Disease/Delete",
        body: {
            'Id_Enfermedad': id,
        },

        cbSuccess: (res) => {
            if (res.result[1]) {

                Swal.fire({

                    icon: 'success',
                    title: 'Bien!',
                    text: res.result[0],
                    timer: 4000,
                    customClass: {
                        container: 'rounded-modal' // Aquí establecemos la clase personalizada para el modal
                    }
                });


                let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
                cargarComponent({
                    container: "diseaseTable",
                    url: "/MedicalConditions/Disease/_TableDiseases",
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
                    text: res.result[0],
                    timer: 4000,

                });
            }

        }
    });




}

