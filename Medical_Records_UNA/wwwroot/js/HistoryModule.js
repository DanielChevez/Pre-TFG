var originalValuesFormFamily = {};
var CurrentValuesFormFamily = {};
document.addEventListener("click", (e) => {
    if (e.target.id === "edit_heredo_familiar") {

        activeFamilyDisease();

    }
    else if (e.target.id === "cancelar_heredo_familiar") {
        disableFamilyDisease();

    }
    else if (e.target.id === "guardar_heredo_familiar") {

        SendDataFamilyDiasease();

        disableFamilyDisease();



        //saveFamilyDiasease();
    }
    else if (e.target.id === "edit_pathological_diseases") {
        activePathologicalDiseases();
        return false;
    }

    else if (e.target.id === "cancelar_pathological_diseases") {
        disablePathologicalDiseases();
        return false;
    }
    else if (e.target.id === "guardar_pathological_diseases") {

        sendDataPathologicalDiseases();

        return false;
    }
    else if (e.target.id === "create_condition") {
        console.log("Entra");
        $("#add_condition_modal").modal('show');
    }
});


document.addEventListener("submit", (e) => {

    e.preventDefault();
    if (e.target.id === "edit_heredo_familiar") {

    }


});

document.addEventListener("change", (e) => {

    if (e.target.id === "edit_heredo_familiar") {

        
    }


    $(".opciones input[type='text']").on("input", function () {
        var id = $(this).data("id").replace("parentesco_", "");
        var checkbox = $(`input[type='checkbox'][data-id='${id}']`);
        checkbox.prop("checked", true);
    });
});

function activePathologicalDiseases() {

    
    document.getElementById("edit_pathological_diseases").setAttribute("hidden", "");

    document.getElementById("cancelar_pathological_diseases").removeAttribute("hidden");
    document.getElementById("guardar_pathological_diseases").removeAttribute("hidden");

    $("#pathologicalDiseases input[type='checkbox']").prop('disabled', false);
    $("#pathologicalDiseases input[type='text']").prop('disabled', false);

    
};
function disablePathologicalDiseases() {

    document.getElementById("edit_pathological_diseases").removeAttribute("hidden");

    document.getElementById("cancelar_pathological_diseases").setAttribute("hidden", "");
    document.getElementById("guardar_pathological_diseases").setAttribute("hidden", "");
    $("#pathologicalDiseases input[type='checkbox']").prop('disabled', true);
    $("#pathologicalDiseases input[type='text']").prop('disabled', true);

    //const form = 
    document.getElementById("form_pathological_disease").reset();
};
function activeFamilyDisease() {
    document.getElementById("edit_heredo_familiar").setAttribute("hidden", "");

    document.getElementById("cancelar_heredo_familiar").removeAttribute("hidden");
    document.getElementById("guardar_heredo_familiar").removeAttribute("hidden");

    $(".opciones input[type='checkbox']").prop('disabled', false);
    $(".opciones input[type='text']").prop('disabled', false);

};

function disableFamilyDisease() {

    document.getElementById("edit_heredo_familiar").removeAttribute("hidden");

    document.getElementById("cancelar_heredo_familiar").setAttribute("hidden", "");
    document.getElementById("guardar_heredo_familiar").setAttribute("hidden", "");
    $(".opciones input[type='checkbox']").prop('disabled', true);
    $(".opciones input[type='text']").prop('disabled', true);

    //const form = 
    document.getElementById("form_heredo_familiar").reset();
};

function SendDataFamilyDiasease() {
    var data = getDataDiseaseFamily();
    console.log(data);
    fetchMethod({
        url: "/MedicalConditions/Disease/SaveFamilyDisease",
        body: data,

        cbSuccess: (res) => {

            //console.log(res);
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


};
function sendDataPathologicalDiseases() {

    var data = getDataPathologicalDiseases();
    console.log(data);
    fetchMethod({
        url: "/MedicalConditions/Disease/SavePathologicalDisease",
        body: data,

        cbSuccess: (res) => {
            console.log(res);
            //console.log(res);
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


            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'No se guardaron los cambios...',
                    timer: 4000,

                })
            }
            disablePathologicalDiseases();
        }
    });


};

function getDataPathologicalDiseases() {
    var checkboxes = document.querySelectorAll("#form_pathological_disease input[type='checkbox']");
    
    
    var id_paciente = document.getElementById("id_paciente").value;
    var diseaseVM = {
        Id_Paciente: id_paciente,
        Tipo: "", // Asigna el valor apropiado
        Diseases: []
    };

    checkboxes.forEach(function (checkbox) {
        var id = checkbox.getAttribute("data-id");
        var checked = checkbox.checked;
        
        diseaseVM.Diseases.push({
            Id_Enfermedad: id,            
            Activo: checked,            
        });
    });

    // Ahora puedes hacer algo con la instancia de diseaseVM, como enviarla a través de una solicitud AJAX
    return diseaseVM;
};
function getDataDiseaseFamily() {
    var checkboxes = document.querySelectorAll("#form_heredo_familiar input[type='checkbox']");
    var textInputs = document.querySelectorAll("#form_heredo_familiar input[type='text']");
    var id_paciente = document.getElementById("id_paciente").value;
    var diseaseVM = {
        Id_Paciente: id_paciente,
        Tipo: "", // Asigna el valor apropiado
        Diseases: []
    };

    checkboxes.forEach(function (checkbox) {
        var id = checkbox.getAttribute("data-id");
        var checked = checkbox.checked;
        var nombre = checkbox.nextElementSibling.textContent;
        var parentesco = document.getElementById("parentesco_" + id)?.value || "";

        diseaseVM.Diseases.push({
            Id_Enfermedad: id,
            Nombre: nombre,
            Activo: checked,
            Parentesco: parentesco
        });
    });

    // Ahora puedes hacer algo con la instancia de diseaseVM, como enviarla a través de una solicitud AJAX
    return diseaseVM;

};

function compareDataDiseaseFamily(original, newData) {

    for (var key in original.Diseases) {
        if (original.Diseases[key] !== newData.Diseases[key]) {
            return true;
        }
    }
    return false;

};

function getDataFormCondition() {


    var data = {

        'Nombre': doc.getElementById("nombre").value,
        'Descripcion': doc.getElementById("descripcion_condicion").value,
        'Estado': doc.getElementById("estado").value,
        'Recomendacion': doc.getElementById("recomendacion_condicion").value,
        'Id_Enfermedad': doc.getElementById("id_condicion").value,
        'Tipo': doc.getElementById("tipo").value
    };


    return data;
}