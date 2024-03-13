
    const dc = document,
        $renderBody = document.getElementById("renderBody");

   

var formElement_personal = document.getElementById("form_personal_info");
var formElement_family = document.getElementById("form_familiar_info");

var originalFormValues_personal = {};
var originalFormValues_family = {};




document.addEventListener("click", (e) => {



        //====================================================
        //======   MODULE PERSONAL INFORMATION   =============
        //====================================================


    //====================================================
    //=============  SHOW VIEW   =========================
    //============  INFORMATION  =========================
    //====================================================

      if (e.target.id === "information") {

            dc.getElementById("modules").setAttribute("hidden", "");
            dc.getElementById("module").removeAttribute("hidden");

            dc.getElementById("ver_modulos").removeAttribute("hidden");
            const id = dc.getElementById("id_paciente").value;

            dc.getElementById("module").name = "_InformationModule";

            //cargarComponent({
            //    container: "module",
            //    url: "/Folders/Folder/_InformationModule",
            //    body: {
            //        Id_Paciente: dc.getElementById("id_paciente").value,
            //    }
            //});
            cargarComponent({
                container: "module",
                /*url: "/Folders/Module/_InformationModule",*/ // ruta original y funcional
                url: "/Modules/Patient_Information/_InformationModule",
                body: {
                    Id_Paciente: dc.getElementById("id_paciente").value,
                }
            });


      }

      else if (e.target.id == "add_info_personal") {
          var data = {
              'Accion': false,
              'Cedula': dc.getElementById("cedula").value,
              'Nombre': dc.getElementById("nombre").value,
              'Apellidos': dc.getElementById("apellidos").value,
              'Email': dc.getElementById("email").value,
              'Fecha_Nacimiento': dc.getElementById("date").value,
              'Sexo': dc.getElementById("sexo").value,
              'Direccion': dc.getElementById("direccion").value,
              'Lugar_origen': dc.getElementById("origen").value,

              'Profesion': dc.getElementById("profesion").value,
              'Estado_civil': dc.getElementById("estado_civil").value,



              'Condicion_Rehabilitacion': dc.getElementById("condicion").value,
              'Numero_Expediente': dc.getElementById("numero_expediente").value,

              'Fase': dc.getElementById("fase").value,

              'Telefono': dc.getElementById("tel").value,




          };
          fetchMethod({
              /*url: "/Patient/Add_Update_Information_Personal",*/ // url original y funcional
              url: "/Modules/Patient_Information/Add_Update_Information_Personal",
              body: data,

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
                      $("#add_patient_modal").modal('hide');


                      let numPage = Number(dc.getElementById("pagActual").textContent) - 1;
                      let objPaginacion = {
                          palabraBuscar: dc.getElementById("buscar").value.toLowerCase(),
                          NumPagina: numPage,
                          CantRegistros: dc.getElementById("tamanoPagina").value,
                          totalPaginas: dc.getElementById("totalPag").textContent,

                          url: "/Folders/Folder/_TableFolders",
                          container: "foldersTable",

                      }

                      cargarComponent({

                          url: "/Folders/Folder/_TableFolders",
                          container: "foldersTable",
                          body: objPaginacion
                      });

                  }
                  else {
                      Swal.fire({
                          icon: 'error',
                          title: 'Oops...',
                          text: 'No se guardaron los cambios...',
                          timer: 4000,

                      })
                      $("#form_personal_info").reset();

                  }
              }
          });

      }
      else if (e.target.id === "edit_info_personal") {

          originalFormValues_personal = getDataFormPersonal();
          activePersonalInformation();
      }
      //====================================================
      //=============  DISABLE DATA =========================
      //================= FOR EDIT ==========================
      //====================================================
      else if (e.target.id === "cancelar_info_personal") {
          disabledPersonalInformation();
      }
      //===================================================
      //========  ACTIVE DATA OF FAMILY ====================
      //================= TO EDIT ==========================
      //====================================================

      else if (e.target.id === "edit_info_familiar") {
          originalFormValues_family = getDataFormFamily();

          activeFamilyInformation();

      }
      //====================================================
      //=============  DISABLE DATA =========================
      //================= FOR EDIT ==========================
      //====================================================
      else if (e.target.id === "cancelar_info_familiar") {

          disabledFamilyInformation();

      }
      //===================================================
      //=============  SAVE PERSONAL DATA ==================
      //================== AFTER EDIT ======================
      //====================================================
      else if (e.target.id === "guardar_info_personal") {


          var newData = getDataFormPersonal();
          var result = compareFormValues(originalFormValues_personal, newData);

          if (compareFormValues(originalFormValues_personal, newData)) {
              sendPersonalData();
          }
          disabledPersonalInformation();
      }
      //===================================================
      //=============  SAVE FAMILY DATA ==================
      //================== AFTER EDIT ======================
      //====================================================
      else if (e.target.id === "guardar_info_familiar") {
          console.log("guardar");
          var newData = getDataFormFamily();

          if (compareFormValues(originalFormValues_family, newData)) {
              sendDataFamily();
          }
          disabledFamilyInformation();
      }
});



    $renderBody.addEventListener("submit", (e) => {





    });


    $renderBody.addEventListener("change", (e) => {





    });
function getDataFormPersonal() {

    var id = dc.getElementById("id_paciente").value || "";

    var data = {
        'Id_Persona': dc.getElementById("id_persona").value,
        'Id_Paciente': id,
        'Accion': true,
        'Cedula': dc.getElementById("cedula").value,
        'Nombre': dc.getElementById("nombre_paciente").value,
        'Apellidos': dc.getElementById("apellidos").value,
        'Email': dc.getElementById("email").value,
        'Fecha_Nacimiento': dc.getElementById("date").value,
        'Sexo': dc.getElementById("sexo").value,
        'Direccion': dc.getElementById("direccion").value,
        'Lugar_origen': dc.getElementById("origen").value,

        'Profesion': dc.getElementById("profesion").value,
        'Estado_civil': dc.getElementById("estado_civil").value,



        'Condicion_Rehabilitacion': dc.getElementById("condicion").value,
        'Numero_Expediente': dc.getElementById("numero_expediente").value,

        'Fase': dc.getElementById("fase").value,

        'Telefono': dc.getElementById("tel").value,




    };
    return data;
}


function getDataFormFamily() {

    var data = {
        'Id_Paciente': dc.getElementById("id_paciente").value,
        'Conyuge': dc.getElementById("conyuge").value,
        'Nombre_Hijo': dc.getElementById("hijo").value,
        'Persona_a_cargo': dc.getElementById("per_cargo").value,
        'Telefono_emergencia': dc.getElementById("tel_emergencia").value,
        'Nombre_Contacto': dc.getElementById("contacto").value,
        'Parentesco': dc.getElementById("parentesco").value,

    };


    return data;
}
function sendPersonalData() {

    var dataUpdate = getDataFormPersonal();

    fetchMethod({
        url: "/Modules/Patient_Information/Add_Update_Information_Personal",
        body: dataUpdate
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

                disabledPersonalInformation();
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

function sendDataFamily() {

    var dataUpdate = getDataFormFamily();

    fetchMethod({
        url: "/Modules/Patient_Information/Add_Update_FamilyInformation",
        body: dataUpdate
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

                disabledFamilyInformation();
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

function activeFamilyInformation() {
    dc.getElementById("edit_info_familiar").setAttribute("hidden", "");

    dc.getElementById("cancelar_info_familiar").removeAttribute("hidden");
    dc.getElementById("guardar_info_familiar").removeAttribute("hidden");

    dc.getElementById("conyuge").removeAttribute("disabled");
    dc.getElementById("hijo").removeAttribute("disabled");
    dc.getElementById("per_cargo").removeAttribute("disabled");
    dc.getElementById("tel_emergencia").removeAttribute("disabled");
    dc.getElementById("contacto").removeAttribute("disabled");
    dc.getElementById("parentesco").removeAttribute("disabled");
};


function activePersonalInformation() {
    dc.getElementById("edit_info_personal").setAttribute("hidden", "");

    dc.getElementById("cancelar_info_personal").removeAttribute("hidden");
    dc.getElementById("guardar_info_personal").removeAttribute("hidden");

    dc.getElementById("nombre_paciente").removeAttribute("disabled");

    dc.getElementById("apellidos").removeAttribute("disabled");
    dc.getElementById("cedula").removeAttribute("disabled");
    dc.getElementById("profesion").removeAttribute("disabled");
    dc.getElementById("estado_civil").removeAttribute("disabled");

    dc.getElementById("date").removeAttribute("disabled");
    dc.getElementById("sexo").removeAttribute("disabled");
    dc.getElementById("condicion").removeAttribute("disabled");
    dc.getElementById("fase").removeAttribute("disabled");
    dc.getElementById("numero_expediente").removeAttribute("disabled");
    dc.getElementById("tel").removeAttribute("disabled");
    dc.getElementById("email").removeAttribute("disabled");
    dc.getElementById("origen").removeAttribute("disabled");
    dc.getElementById("direccion").removeAttribute("disabled");

}

function disabledPersonalInformation() {

    dc.getElementById("edit_info_personal").removeAttribute("hidden");

    dc.getElementById("cancelar_info_personal").setAttribute("hidden", "");
    dc.getElementById("guardar_info_personal").setAttribute("hidden", "");


    dc.getElementById("nombre_paciente").setAttribute("disabled", "");

    dc.getElementById("apellidos").setAttribute("disabled", "");
    dc.getElementById("cedula").setAttribute("disabled", "");
    dc.getElementById("profesion").setAttribute("disabled", "");
    dc.getElementById("estado_civil").setAttribute("disabled", "");

    dc.getElementById("date").setAttribute("disabled", "");
    dc.getElementById("sexo").setAttribute("disabled", "");
    dc.getElementById("condicion").setAttribute("disabled", "");
    dc.getElementById("fase").setAttribute("disabled", "");
    dc.getElementById("numero_expediente").setAttribute("disabled", "");
    dc.getElementById("tel").setAttribute("disabled", "");
    dc.getElementById("email").setAttribute("disabled", "");
    dc.getElementById("origen").setAttribute("disabled", "");
    dc.getElementById("direccion").setAttribute("disabled", "");

}

function disabledFamilyInformat() {
    dc.getElementById("edit_info_familiar").removeAttribute("hidden");

    dc.getElementById("cancelar_info_familiar").setAttribute("hidden", "");
    dc.getElementById("guardar_info_familiar").setAttribute("hidden", "");

    dc.getElementById("conyuge").setAttribute("disabled", "");
    dc.getElementById("hijo").setAttribute("disabled", "");
    dc.getElementById("per_cargo").setAttribute("disabled", "");
    dc.getElementById("tel_emergencia").setAttribute("disabled", "");
    dc.getElementById("contacto").setAttribute("disabled", "");
    dc.getElementById("parentesco").setAttribute("disabled", "");
}