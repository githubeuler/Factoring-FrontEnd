﻿'use strict';
var globalPath = $('base').attr('href');
var acciones = [
 {
        "NOMBRE": "SIN-ACCION",
        "CODIGO": 1,
        "CLASE" : ".sa"
    },
 {
        "NOMBRE": "INSERTAR",
        "CODIGO": 2,
        "CLASE" : ".p-ins"
    },
 {
        "NOMBRE": "ELIMINAR",
        "CODIGO": 3,
        "CLASE": ".p-eli"
    },
 {
        "NOMBRE": "ACTUALIZAR",
        "CODIGO": 4,
        "CLASE": ".p-act"
    },
 {
        "NOMBRE": "CONSULTAR",
        "CODIGO": 5,
        "CLASE": ".p-con"
    },
{
        "NOMBRE": "EXPORTAR_EXCEL",
        "CODIGO": 6,
        "CLASE": ".p-exp-exc"
    },
 {
        "NOMBRE": "EXPORTAR_PDF",
        "CODIGO": 7,
        "CLASE": ".p-exp-pdf"
    },
{
        "NOMBRE": "REVERTIR",
        "CODIGO": 8,
        "CLASE": ".p-rev"
    },
 {
        "NOMBRE": "TRANSFERIR",
        "CODIGO": 9,
        "CLASE": ".p-transfe"
    },
 {
        "NOMBRE": "DESCARGAR",
        "CODIGO": 10,
        "CLASE": ".p-des"
    },
 {
        "NOMBRE": "EVALUAR",
        "CODIGO": 11,
        "CLASE": ".p-eva"
    },
{
        "NOMBRE": "ANULAR",
        "CODIGO": 12,
        "CLASE": ".p-anu"
    },
 {
        "NOMBRE": "CARGAR",
        "CODIGO": 13,
        "CLASE": ".p-car"
    },
 {
        "NOMBRE": "CERRAR",
        "CODIGO": 14,
        "CLASE": ".p-cer"
    },
 {
        "NOMBRE": "OBSERVAR",
        "CODIGO": 15,
        "CLASE": ".p-obs"
    },
 {
        "NOMBRE": "RECHAZAR",
        "CODIGO": 16,
        "CLASE": ".p-rec"
    },
 {
        "NOMBRE": "APROBAR",
        "CODIGO": 17,
        "CLASE": ".p-apr"
    },
{
        "NOMBRE": "REMOVER",
        "CODIGO": 18,
        "CLASE": ".p-rem"
    },
 {
        "NOMBRE": "TRASPASAR",
        "CODIGO": 19,
        "CLASE": ".p-traspa"
    }, 
];
var Common = function () {
    var HideShowButtom = function () {

        var _menu = JSON.parse($('#idMenu').val());

        var _pathname = '';
        if (window.location.href.includes('localhost')) {
            _pathname = window.location.pathname.substring(1)
        } else {
            var split = window.location.pathname.substring(1).split('/')
            //_pathname = split[split.length - 2] + '/' + split[split.length - 1];
            _pathname = split.length == 3 ? split[split.length - 2] + '/' + split[split.length - 1] : split[split.length - 1];
        }
        console.log(_pathname)
        var men_per = ''
        var _menuItem = null;
       
        for (var i = 0; i < _menu.length; i++) {
            var cc = _menu[i].cUrl.includes('/Index') ? _menu[i].cUrl.replace('/Index', '') : _menu[i].cUrl
            var dd = _pathname
            if (cc == dd) {
                men_per = _menu[i].cMenuPermisos;
                _menuItem = _menu[i]
            }
        }

        console.log('men_per', men_per)
        console.log('_menuItem', _menuItem)

        if (_menuItem == undefined) {
            return false;
        }

        var _menuHijo = _menu.filter(item => item.nNivel === _menuItem.nIdMenu)

        console.log('_menuHijo', _menuHijo)

        for (var i = 0; i < acciones.length; i++) {
            Common.validarElemento(men_per, acciones[i].CODIGO, acciones[i].CLASE)
        }

        for (var i = 0; i < _menuHijo.length; i++) {
            if (!_menuHijo[i].cUrl.includes('/')) {
                for (var i = 0; i < acciones.length; i++) {
                    Common.validarElementoTab(_menuHijo[i].cUrl, _menuHijo[i].cMenuPermisos, acciones[i].CODIGO, acciones[i].CLASE);
                }
            }
        }
    }
    return {
        init: function () {
            HideShowButtom();
            
        },
        validarElemento: function (menu, accion, clase) {
            var elem_consulta = $(clase);
            if (menu.indexOf(accion) == -1) {
                elem_consulta.hide()
            }
        },
        validarElementoTab: function (div, menu, accion, clase) {
            debugger;
            var elem_consulta = $("#" + div).find(clase);
            if (menu.indexOf(accion) == -1) {
                $(elem_consulta).hide()
            } else {
                $(elem_consulta).show()
            }
        },

    }
}();
KTUtil.onDOMContentLoaded(function () {
    Common.init();
});