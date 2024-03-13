


const doc = document,
    $rendBody = doc.getElementById("renderBody");
 


$rendBody.addEventListener("click", (e) => {

    let page = e.target.dataset.value || "";

    //====================================================
    //======   MODULE PERSONAL INFORMATION   =============
    //====================================================
    if (e.target.id === "records") {
        doc.getElementById("modules").setAttribute("hidden", "");
        doc.getElementById("module").removeAttribute("hidden");

        doc.getElementById("ver_modulos").removeAttribute("hidden");
        const id = doc.getElementById("id_paciente").value;

        doc.getElementById("module").name = "_HistoryModule";

   
        var idPa = doc.getElementById("id_paciente").value;


        //cargarComponentAsync({
        //    container: "module",
        //    url: "/Folders/Module/_HistoryModule",
        //    body: {
        //        Id_Paciente: doc.getElementById("id_paciente").value,
        //    }
        //});
        cargarComponentAsync({
            container: "module",
            url: "/Modules/History/_HistoryModule",
            body: {
                Id_Paciente: doc.getElementById("id_paciente").value,
            }
        });

   
    }

    //====================================================
    //======   MODULE PERSONAL INFORMATION   =============    
    //====================================================
    else if (e.target.id === "create_patient") {
        $("#add_patient_modal").modal('show');
    }


    //====================================================
    //=============  RETURN TO VIEW ======================
    //===============  OF MODULES  =======================
    //====================================================

    else if (e.target.id === "ver_modulos") {
        doc.getElementById("ver_modulos").setAttribute("hidden", "");
        doc.getElementById("module").setAttribute("hidden", "");
        doc.getElementById("modules").removeAttribute("hidden");



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
            url: "/Folders/Folder/_TableFolders",
            container: "foldersTable",

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
            url: "/Folders/Folder/_TableFolders",
            container: "foldersTable",
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase()




        });

    }

});



$rendBody.addEventListener("submit", (e) => {

    e.preventDefault();

    

});


$rendBody.addEventListener("change", (e) => {

    let page = e.target.dataset.value || "";



    $("#buscar").on("keyup", function () {





        let numPage = Number(doc.getElementById("pagActual").textContent) - 1;
        let objPaginacion = {
            palabraBuscar: doc.getElementById("buscar").value.toLowerCase(),
            NumPagina: numPage,
            CantRegistros: doc.getElementById("tamanoPagina").value,
            totalPaginas: doc.getElementById("totalPag").textContent,

            url: "/Folders/Folder/_TableFolders",
            container: "foldersTable",

        }

        cargarComponent({

            url: "/Folders/Folder/_TableFolders",
            container: "foldersTable",
            body: objPaginacion
        });

    });

});


async function getTypesDisease() {

    const checks = doc.getElementById("disease_check");
fetchMethod({
        url: "/MedicalConditions/Disease/getTypes",
        cbSuccess: (res) => {
            
            
            var list = createChecklist(res.disease);
            const disease_checkList = $('#disease_check');

            //disease_checkList.append(crearChecklist(enfermedades));
            createChecklist(enfermedades);
        }
    });


};
