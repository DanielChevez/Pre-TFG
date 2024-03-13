//const fetchMethod = (props) => {

//    let { url, method = "POST", body, cbSuccess } = props;

//    fetch(url, {
//        method: method,
//        headers: {
//            //"Accept": "application/json",
//            //"Content-Type": "application/json"
//            'Content-Type': 'application/json'
//        },
//        body: JSON.stringify(body)

//    }).then(res => (res.ok ? res.json() : Promise.reject(res)))
//        .then(data => cbSuccess(data))
//        .catch(error => console.error(error));
//};

const fetchMethod = (props) => {

    let { url, method = "POST", body, cbSuccess } = props;

    fetch(url, {
        method: method,
        headers: {
            //"Accept": "application/json",
            //"Content-Type": "application/json"
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(body)

    }).then(res => (res.ok ? res.json() : null))
        .then(res => cbSuccess(res))
        .catch(error => console.error(error));
};

const cargarComponent = (props) => {
    let { container, url, body } = props;
    fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)

    })
        .then((res) => res.text())
        .then((viewPartial) => document.getElementById(container).innerHTML = viewPartial)
        .catch((er) => console.error("Ha ocurrido un error al cargar el contenido", er));
};


  async function cargarComponentAsync (props) {
    let { container, url, body } = props;
    await fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(body)

    })
        .then((res) => res.text())
        .then((viewPartial) => document.getElementById(container).innerHTML = viewPartial)
        .catch((er) => console.error("Ha ocurrido un error al cargar el contenido", er));
};

//Pasar a la pagina siguiente
const siguientePagina = (props) => {
    let { idRelacion, sig, totalPage, CantRegistros, url, container, palabraBuscar } = props;
    let siguiente = sig - 1;//Limpiamos el valor que viene de la vista
    fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Usuario: idRelacion,
            NumPagina: siguiente,
            palabraBuscar: "",
            accion: 'S',
            totalPaginas: totalPage,
            CantRegistros: CantRegistros,
            palabraBuscar: palabraBuscar
        })
    })
        .then((res) => res.text())
        .then((viewPartial) => document.getElementById(container).innerHTML = viewPartial)
        .catch((er) => console.error("Ha ocurrido un error al cargar el contenido", er));
};


//Pasar a la pagina anterior
const retrocederPagina = (props) => {
    let { idRelacion, ant, totalPage, tamanoPagina, url, container, palabraBuscar } = props;
    var anterior = ant - 1;//Limpiamos el valor que viene de la vista
    fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Usuario: idRelacion,
            NumPagina: anterior,
            palabraBuscar: palabraBuscar,
            accion: 'N',
            totalPaginas: totalPage,
            CantRegistros: tamanoPagina
        })
    })
        .then((res) => res.text())
        .then((partialView) => document.getElementById(container).innerHTML = partialView)
        .catch((er) => console.error("Ha ocurrido un error al cargar el contenido", er));
};

// Cambio de tamaño de paginacion
const cambioTamanoPagina = (props) => {
    let { idRelacion, totalPage, tamanoPagina, url, container, palabraBuscar, NumPagina } = props;

    fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Usuario: idRelacion,
            palabraBuscar: palabraBuscar,
            totalPaginas: totalPage,
            CantRegistros: tamanoPagina,
            NumPagina
        })
    })
        .then((res) => res.text())
        .then((partialView) => document.getElementById(container).innerHTML = partialView)
        .catch((er) => console.error("Ha ocurrido un error al cargar el contenido", er));

};

const buscarPalabra = (props) => {
    let { idRelacion, url, container, palabraBuscar, tamanoPagina } = props;
    fetch(url, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Usuario: idRelacion,
            NumPagina: 0,
            palabraBuscar: palabraBuscar,
            estaBuscando: true,
            accion: 'n', //n -> no pasar de pagina
            CantRegistros: tamanoPagina,

        })
    })
        .then((res) => res.text())
        .then((partialView) => document.getElementById(container).innerHTML = partialView)
        .catch((er) => console.error("Ha ocurrido un error al cargar el contenido", er));
};

function limpiarHtml(elemento) {

    while (elemento.firstChild) {
        elemento.removeChild(elemento.firstChild);
    }
};


function getFormValues(form) {
    var formData = {};

    for (var i = 0; i < form.elements.length; i++) {
        var element = form.elements[i];
        if (element.type !== "submit" && element.type !== "button") {
            if (element.type === "checkbox") {
                formData[element.id] = element.checked;
            } else {
                formData[element.id] = element.value;
            }
        }
    }

    return formData;
}

function compareFormValues(originalValues, currentValues) {
    
    for (var key in originalValues) {
        if (originalValues[key] !== currentValues[key]) {
            return true;
        }
    }
    return false;
}




//function alertDelete(title, text) {

//    const swalWithBootstrapButtons = Swal.mixin({
//        customClass: {
//            confirmButton: 'btn btn-success  ',
//            cancelButton: 'btn btn-danger buttonModal'
//        },
//        buttonsStyling: false
//    })

//    swalWithBootstrapButtons.fire({
//        title: title,
//        text: text,
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonText: 'Sí,Eliminar!',
//        cancelButtonText: 'Cancelar!',
//        reverseButtons: true
//    }).then((result) => {
//        if (result.isConfirmed) {
//            return true;
//        }
//        else if (result.dismiss === Swal.DismissReason.cancel) {
//            swalWithBootstrapButtons.fire(
//                'Cancelado',
//                'Acción Cancelada :)',
//                'error',
//                4000,
//            )
//            return false;

//        }

//    })


//}
function alertDelete(title, text) {
    return new Promise((resolve, reject) => {
        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success',
                cancelButton: 'btn btn-danger buttonModal'
            },
            buttonsStyling: false
        });

        swalWithBootstrapButtons.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, Eliminar!',
            cancelButtonText: 'Cancelar',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                resolve(true);
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                swalWithBootstrapButtons.fire(
                    'Cancelado',
                    'Acción Cancelada :)',
                    'error'
                );
                resolve(false);
            }
        });
    });
}
