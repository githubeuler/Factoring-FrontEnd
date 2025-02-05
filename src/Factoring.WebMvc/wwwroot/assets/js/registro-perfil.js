'use strict';
var globalPath = $('base').attr('href');
var Perfil = function () {
    var datatable;
    var datatableRol;
    var datatableAcciones;
    var initDatatable = function () {
        var table = document.getElementById('kt_rol_table');
        if (!table) {
            return;
        }
        $(table).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatable = $(table).DataTable({
            searchDelay: 500,
            processing: true,
            serverSide: true,
            stateSave: false,
            ordering: false,
            ajax: {
                url: globalPath + 'Perfil/ListadoRegistros',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdRoles', name: 'nIdRoles', 'autoWidth': true, class: 'text-center' },
                { data: 'cNombreRol', 'autoWidth': true, class: 'text-center' },
                { data: 'cDesEstado', 'autoWidth': true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    className: 'text-center',
                    render: function (data, type, row) {
                        var checkbox = `<div class="form-check form-check-sm form-check-custom form-check-solid"><input class="form-check-input checkbox-main" type="checkbox" value="${row.nIdRoles}" /></div>`;
                        return (row.iEstadoFile == 1) ? `` : checkbox;
                    }
                },
                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-center',
                    render: function (data, type, row) {
                        var buttonAction = `<div style="display:inline-flex"><a href="${globalPath}Perfil/Registro?nRolId=${data.nIdRoles}&nOpcion=1" title="Editar" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2"><i class="las la-pen fs-2"></i></a>
                        <button data-delete-table="delete_row" data-id="` + row.nIdRoles + `" data-row= ${data.nIdRoles} title="Eliminar"  class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-eli"><i class="las la-ban fs-2"></i></button>
                        <div style="display:inline-flex"><a href="${globalPath}Perfil/Registro?nRolId=${data.nIdRoles}&nOpcion=2" title="Ver" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2"><i class="las la-search fs-2"></i></a>`;
                        return buttonAction;
                    }
                }
            ]
        });
        table = datatable.$;
        datatable.on('draw', function () {
            var searchButton = document.getElementById('kt_search_button');
            var searchClear = document.getElementById('kt_search_clear');
            searchButton.removeAttribute('data-kt-indicator');
            searchButton.disabled = false;
            handleDeletePerfilForm();
            $(searchClear).show();
            KTMenu.createInstances();
        });
    }
    var handleFilterTable = function () {
        var searchButton = document.getElementById('kt_search_button');
        if (!searchButton) {
            return;
        }
        searchButton.addEventListener('click', function (e) {
            e.preventDefault();
            $(searchClear).hide();
            searchButton.setAttribute('data-kt-indicator', 'on');
            searchButton.disabled = true;
            setTimeout(function () {
                searchButton.removeAttribute('data-kt-indicator');
                searchButton.disabled = false;
                $(searchClear).show();
                initDatatable();
            }, 2000);
        });
        var searchClear = document.getElementById('kt_search_clear');
        if (!searchClear) {
            return;
        }
        searchClear.addEventListener('click', function (e) {
            e.preventDefault();
            $(this).closest('form').find('input[type=text], textarea').val('');
            initDatatable();
        });
    }



    var handleRegistroPerfil = function () {
        var formRegistroPerfil = document.getElementById('kt_form_add');
        if (!formRegistroPerfil) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button');
        var validator;
        validator = FormValidation.formValidation(
            formRegistroPerfil,
            {
                fields: {
                    'cNombreRol': {
                        validators: {
                            notEmpty: {
                                message: 'Nombre del rol es obligatorio'
                            }
                        }
                    }
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleValidClass: '',
                        eleInvalidClass: '',
                    })
                }
            }
        );
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();

            // Validar el formulario
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    var request = {
                        cNombreRol: $("#cNombreRol").val()
                    };

                    // Activar indicador de carga
                    saveButton.setAttribute('data-kt-indicator', 'on');
                    saveButton.disabled = true;

                    // Enviar solicitud AJAX
                    $.ajax({
                        type: 'POST',
                        url: globalPath + 'Perfil/RegistrarPerfil',
                        contentType: 'application/json',
                        data: JSON.stringify(request),
                        success: function (data) {
                            saveButton.removeAttribute('data-kt-indicator');
                            saveButton.disabled = false;

                            if (data.succeeded) {
                                Swal.fire({
                                    text: data.message,
                                    icon: 'success',
                                    buttonsStyling: false,
                                    confirmButtonText: 'Listo',
                                    customClass: {
                                        confirmButton: 'btn fw-bold btn-primary',
                                    }
                                }).then(function () {
                                    // $(window).attr('location', globalPath + 'Perfil');
                                    //nIdRol
                                    console.log('data.datadata.data', data.data);
                                    $("#nIdRolAccion").val(data.data);
                                    $('#dvListaModulo').show();

                                });
                            } else {
                                messageError(data.message);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            saveButton.removeAttribute('data-kt-indicator');
                            saveButton.disabled = false;
                            messageError(errorThrown);
                        }
                    });
                }
            });
        });
    };













    var initDataTableRol = function () {
        var nIdRol = document.getElementById('nIdRol');
        const element = document.getElementById("aBuscarGrilla");
        const hbuscar = element ? element.value : null;
        if (hbuscar === "1") {
            $('#dvListaModulo').hide();
        }
        if (hbuscar != "1" && hbuscar != "2" && hbuscar != "3") {
            return;
        }
        if (hbuscar == "3") {
            document.getElementById("cNombreRol").disabled = true;
            document.getElementById("kt_save_button").disabled = true;
        }

        var tableMenu = document.getElementById('kt_menu_table');
        if (!tableMenu) {
            return;
        }

        $(tableMenu).DataTable({ ordering: false }).clear().destroy();
        $.fn.dataTable.ext.errMode = 'none';
        datatableRol = $(tableMenu).DataTable({
            ordering: false,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Perfil/GetAllMenu?nIdRol=' + $(nIdRol).val(),
                dataSrc: function (data) {
                    return data.data;
                }
            },
            columns: [
                { data: 'nIdMenuDetalle', autoWidth: true, class: 'text-center' },
                { data: 'cModulo', autoWidth: true, class: 'text-center' },
                { data: 'cMenu', autoWidth: true, class: 'text-center' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
                //{ data: 'cMenu', autoWidth: true, class: 'text-center' },


                //{
                //    data: 'cInsertar',
                //    autoWidth: true,
                //    class: 'text-center',
                //    render: function (data, type, row) {
                //        return `<input type="checkbox" class="checkbox-create" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                //    }
                //},
                //{
                //    data: 'cActualizar',
                //    autoWidth: true,
                //    class: 'text-center',
                //    render: function (data, type, row) {
                //        return `<input type="checkbox" class="checkbox-update" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                //    }
                //},
                //{
                //    data: 'cConsultar',
                //    autoWidth: true,
                //    class: 'text-center',
                //    render: function (data, type, row) {
                //        return `<input type="checkbox" class="checkbox-read" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                //    }
                //},
                //{
                //    data: 'cEliminar',
                //    autoWidth: true,
                //    class: 'text-center',
                //    render: function (data, type, row) {
                //        return `<input type="checkbox" class="checkbox-delete" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                //    }
                //}



            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        var buttonAction = ``;
                        if (hbuscar == "1" || hbuscar == "2") {
                            if (data.nExiste > 0) {
                                buttonAction += `<div style="display:inline-flex">                               
                             <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_menu_acciones"  data-n-nIdRolMenuAccion=${data.nIdRolMenuAccion} data-n-menu=${data.nIdMenuDetalle} data-n-rol=${$(nIdRol).val()} title="Editar"><i class="las la-pen fs-2"></i></a>
                        </div>`
                            } else {
                                buttonAction += `<div style="display:inline-flex">                            
                        <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_menu_acciones"  data-n-nIdRolMenuAccion=${data.nIdRolMenuAccion} data-n-menu=${data.nIdMenuDetalle} data-n-rol=${$(nIdRol).val()} title="Agregar"><i class="la la-chevron-circle-right fs-2"></i></a>

                        </div>`
                            }
                        }
                        else {
                            buttonAction += `<div style="display:inline-flex">                            
                        <a href="javascript:;" class="btn btn-icon btn-light-dark btn-sm open-modal p-eva" data-bs-toggle="modal" data-bs-target="#kt_modal_menu_acciones"  data-n-nIdRolMenuAccion=${data.nIdRolMenuAccion} data-n-menu=${data.nIdMenuDetalle} data-n-rol=${$(nIdRol).val()} title="Agregar"><i class="las la-search fs-2"></i></a>

                        </div>`
                        }
                        return buttonAction;
                    }
                }
            ]

        });
        datatableRol.on('draw', function () {
            Common.init();
            //if (hbuscar == "3") {
            //    document.querySelectorAll("#kt_menu_table input[type='checkbox']").forEach(element => {
            //        element.disabled = true;
            //    });
            //}
        });
    };
    //var getSelectedMenuDetails = function () {
    //    var selectedData = [];
    //    $('#kt_menu_table tbody tr').each(function () {
    //        var nIdMenuDetalle = +$(this).find('td:first').text();
    //        var nIdMenu = +$(this).find('.checkbox-create, .checkbox-update, .checkbox-read, .checkbox-delete').data('menu');
    //        if ($(this).find('.checkbox-create:checked').length > 0 ||
    //            $(this).find('.checkbox-update:checked').length > 0 ||
    //            $(this).find('.checkbox-read:checked').length > 0 ||
    //            $(this).find('.checkbox-delete:checked').length > 0) {
    //            selectedData.push({
    //                nIdMenuDetalle: nIdMenuDetalle,
    //                nIdMenu: nIdMenu,
    //                Insertar: $(this).find('.checkbox-create:checked').length > 0,
    //                Actualizar: $(this).find('.checkbox-update:checked').length > 0,
    //                Consultar: $(this).find('.checkbox-read:checked').length > 0,
    //                Eliminar: $(this).find('.checkbox-delete:checked').length > 0
    //            });
    //        }
    //    });
    //    return selectedData;
    //};
    //var handleRegistroPerfil = function () {
    //    var formRegistroPerfil = document.getElementById('kt_form_add');
    //    if (!formRegistroPerfil) {
    //        return;
    //    }
    //    var saveButton = document.getElementById('kt_save_button');
    //    var validator;
    //    validator = FormValidation.formValidation(
    //        formRegistroPerfil,
    //        {
    //            fields: {
    //                'cNombreRol': {
    //                    validators: {
    //                        notEmpty: {
    //                            message: 'Nombre del rol es obligatorio'
    //                        }
    //                    }
    //                }
    //            },
    //            plugins: {
    //                trigger: new FormValidation.plugins.Trigger(),
    //                bootstrap: new FormValidation.plugins.Bootstrap5({
    //                    rowSelector: '.fv-row',
    //                    eleValidClass: '',
    //                    eleInvalidClass: '',
    //                })
    //            }
    //        }
    //    );
    //    saveButton.addEventListener('click', function (e) {
    //        e.preventDefault();

    //        // Validar el formulario
    //        validator.validate().then(function (status) {
    //            if (status == 'Valid') {
    //                // Obtener datos seleccionados
    //                var selectedData = getSelectedMenuDetails();

    //                // Validar si hay datos seleccionados
    //                if (selectedData.length === 0) {
    //                    Swal.fire({
    //                        text: 'No hay datos seleccionados para guardar.',
    //                        icon: 'warning',
    //                        buttonsStyling: false,
    //                        confirmButtonText: 'Ok',
    //                        customClass: {
    //                            confirmButton: 'btn fw-bold btn-primary',
    //                        }
    //                    });
    //                    return;
    //                }

    //                // Preparar el objeto de solicitud
    //                var request = {
    //                    cNombreRol: $("#cNombreRol").val(),
    //                    ListaMenu: selectedData
    //                };

    //                // Activar indicador de carga
    //                saveButton.setAttribute('data-kt-indicator', 'on');
    //                saveButton.disabled = true;

    //                // Enviar solicitud AJAX
    //                $.ajax({
    //                    type: 'POST',
    //                    url: globalPath + 'Perfil/RegistrarPerfil',
    //                    contentType: 'application/json',
    //                    data: JSON.stringify(request),
    //                    success: function (data) {
    //                        saveButton.removeAttribute('data-kt-indicator');
    //                        saveButton.disabled = false;

    //                        if (data.succeeded) {
    //                            Swal.fire({
    //                                text: data.message,
    //                                icon: 'success',
    //                                buttonsStyling: false,
    //                                confirmButtonText: 'Listo',
    //                                customClass: {
    //                                    confirmButton: 'btn fw-bold btn-primary',
    //                                }
    //                            }).then(function () {
    //                                $(window).attr('location', globalPath + 'Perfil');
    //                            });
    //                        } else {
    //                            messageError(data.message);
    //                        }
    //                    },
    //                    error: function (jqXHR, textStatus, errorThrown) {
    //                        saveButton.removeAttribute('data-kt-indicator');
    //                        saveButton.disabled = false;
    //                        messageError(errorThrown);
    //                    }
    //                });
    //            }
    //        });
    //    });
    //};

    var handleDeletePerfilForm = function () {
        var tableRol = document.querySelector('#kt_rol_table');
        if (!tableRol) {
            return;
        }
        var deleteRolButton = tableRol.querySelectorAll('[data-delete-table="delete_row"]');
        deleteRolButton.forEach(d => {
            d.addEventListener('click', function (e) {
                e.preventDefault();
                debugger;
                var nidRol = $(this).data('id');
                var parent = e.target.closest('tr');
                console.log("nidRolnidRol", nidRol);
                console.log("parent", parent);
                var name = parent.querySelectorAll('td')[1].innerText;
                Swal.fire({
                    text: '¿Estás seguro de que quieres eliminar a ' + name + '?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, eliminar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-danger',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {
                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Perfil/EliminarPerfil',
                            data: {
                                idRol: nidRol
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                if (data) {
                                    Swal.fire({
                                        text: 'Eliminado correctamente a ' + name + '.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        initDatatable();
                                    });
                                } else {
                                   messageError(name + ' hubo un error al eliminar.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        //messageError(name + ' no fue inactivado.');
                    }
                });
            });
        });
    }


    $('#kt_modal_menu_acciones').on('show.bs.modal', function (event) {

        const element = document.getElementById("aBuscarGrilla");
        const hbuscar = element ? element.value : null;
        console.log("modal_hbuscar", hbuscar);
        var button = $(event.relatedTarget);
        var nIdMenu = button.data('n-menu');
        var nIdRol = button.data('n-rol');

        $('#nIdMenuAccion').val(nIdMenu);
        $('#nIdRolAccion').val(nIdRol);
        var tableAcciones = $('#kt_acciones_table');

        if (tableAcciones.length === 0) {
            console.error('No se encontró la tabla kt_acciones_table.');
            return;
        }

        if (hbuscar === "3") {
            document.getElementById("kt_save_acciones_button").disabled = true;
        }
        else {
            document.getElementById("kt_save_acciones_button").disabled = false;
        }
        // Destruir DataTable si ya está inicializado
        if ($.fn.DataTable.isDataTable(tableAcciones)) {
            tableAcciones.DataTable().clear().destroy();
        }

        $.fn.dataTable.ext.errMode = 'none';

        var datatableAcciones = tableAcciones.DataTable({
            ordering: false,
            paging: false,
            scrollCollapse:true,
            ajax: {
                type: 'GET',
                dataType: 'json',
                url: globalPath + 'Perfil/GetAllMenuAcciones',
                data: {
                    nIdRol: nIdRol,
                    nIdMenu: nIdMenu
                },
                dataSrc: function (data) {
                    return data && data.data ? data.data : [];
                }
            },
            columns: [
                { data: 'nIdAccion', autoWidth: true, class: 'text-center' },
                { data: 'cNombreAccion', autoWidth: true, class: 'text-center' },
                { data: null, autoWidth: true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        /*return '<input type="checkbox" ' + (data.nExiste ? 'checked' : '') + ' />';*/
                        return '<input type="checkbox" class="checkbox-accion" data-id="' + row.nIdAccion + '" ' + (data.nExiste ? 'checked' : '') + ' />';
                    }
                }
            ]
        });

        // Ejecutar lógica solo cuando la tabla haya sido renderizada
        datatableAcciones.on('draw', function () {
           
            console.log("Tabla cargada correctamente.");

            if (typeof Common !== 'undefined' && typeof Common.init === 'function') {
                Common.init();
                if (hbuscar == "3") {
                    document.querySelectorAll("#kt_acciones_table input[type='checkbox']").forEach(element => {
                        element.disabled = true;
                    });
                }
            }
            handleModalControlAsignacion();
            //handleModalControlAcciones(); // Se ejecuta después de que la tabla haya sido cargada
        });
    });

    //$('#kt_modal_menu_acciones').on('show.bs.modal', function (event) {
    //    var button = $(event.relatedTarget); // Botón que abrió el modal
    //    var nIdMenu = button.data('n-menu');
    //    var nIdRol = button.data('n-rol');
    //    $('#nIdMenu').val(nIdMenu);
    //    $('#nIdRol').val(nIdRol);
    //    var tableMenu = document.getElementById('kt_acciones_table');
    //    if (!tableMenu) {
    //        return;
    //    }
    //    $(tableMenu).DataTable({ ordering: false }).clear().destroy();
    //    $.fn.dataTable.ext.errMode = 'none';
    //    datatableAcciones = $(tableMenu).DataTable({
    //        ordering: false,
    //        ajax: {
    //            type: 'GET',
    //            dataType: 'json',
    //            url: globalPath + 'Perfil/GetAllMenuAcciones?nIdRol=' + $(nIdRol).val() + '&nIdMenu=' + $(nIdMenu).val(),
    //            dataSrc: function (data) {
    //                return data.data;
    //            }
    //        },
    //        columns: [
    //            { data: 'nIdAccion', autoWidth: true, class: 'text-center' },
    //            { data: 'cNombreAccion', autoWidth: true, class: 'text-center' },
    //            { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
    //        ]

    //    });
    //    datatableAcciones.on('draw', function () {
    //        Common.init();
    //    });

    //});






    //var getSelectedAcciones = function () {
    //    var selectedAcciones = [];
    //    $('#kt_acciones_table tbody tr').each(function () {
    //        var checkbox = $(this).find('.checkbox-accion:checked');
    //        if (checkbox.length > 0) {
    //            selectedAcciones.push({
    //                nIdAccion: checkbox.data('id')
    //            });
    //        }
    //    });
    //    return selectedAcciones;
    //};
    $(document).off('change', '.checkbox-accion').on('change', '.checkbox-accion', function () {
        console.log("Checkbox cambiado:", $(this).data('id'));
        getSelectedAcciones();
    });

    document.addEventListener("DOMContentLoaded", function () {
        const selectAllCheckbox = document.getElementById("select_all");
       

        selectAllCheckbox.addEventListener("change", function () {
            const checkboxes = document.querySelectorAll(".checkbox-accion");
            console.log('entroooo')
            checkboxes.forEach(checkbox => {
                console.log('aaaaaaaaaaaa')
                checkbox.checked = selectAllCheckbox.checked;
                getSelectedAcciones();
            });
        });
    });

    var getSelectedAcciones = function () {
        var selectedAcciones = [];

        $('#kt_acciones_table tbody tr').each(function () {
            var checkbox = $(this).find('.checkbox-accion:checked');

            if (checkbox.length > 0) {
                var nIdRolMenuAccion = parseInt($(this).data('rolmenuaccion'), 10) || 0;
                var nIdAccion = parseInt(checkbox.data('id'), 10) || 0;

                selectedAcciones.push({
                    nIdRolMenuAccion: nIdRolMenuAccion,
                    nIdAccion: nIdAccion,
                    bAccion: true
                });
            }
        });
        if (selectedAcciones.length > 0) {
            var accionesString = selectedAcciones.map(item => item.nIdAccion).join(',');            
            $('#cIdAccion').val(accionesString);
        }
            
        console.log("Lista de acciones seleccionadas:", selectedAcciones);
        $('#ListaAcciones').val(selectedAcciones);
    };


    var handleModalControlAsignacion = function () {
        var table = document.getElementById('kt_acciones_table');
        if (!table) {
            return;
        }

        var form = document.getElementById('kt_modal_menu_acciones_form');
        if (!form) {
            return;
        }

        var saveButton = $('#kt_save_acciones_button');
        var validator;

        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'nIdMenu': {
                        validators: {
                            notEmpty: {
                                message: 'Menu es obligatorio'
                            }
                        }
                    },
                },
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    bootstrap: new FormValidation.plugins.Bootstrap5({
                        rowSelector: '.fv-row',
                        eleValidClass: '',
                        eleInvalidClass: '',
                    })
                }
            });
        //RegistroOperacion.getRevalidateFormElement(form, 'nIdEstadoEvaluacion', validator);
        //RegistroOperacion.getRevalidateFormElement(form, 'cComentario', validator);
        saveButton.off('click');
        saveButton.on('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    saveButton.attr('data-kt-indicator', 'on');
                    saveButton.prop('disabled', true);
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $(form).attr('action'),
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $(form).serializeObject(),
                            success: function (data) {
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: data.message,
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn btn-primary'
                                        }
                                    }).then(function (result) {
                                        if (result.isConfirmed) {
                                            $(window).attr('location', globalPath + 'Perfil');
                                        }
                                    });
                                } else {
                                    saveButton.removeAttr('data-kt-indicator');
                                    saveButton.prop('disabled', false);
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttr('data-kt-indicator');
                                saveButton.prop('disabled', false);
                                messageError(errorThrown);
                            }
                        });
                    }, 2000);
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });
        });
    }


    //var handleRegistroPerfil = function () {

    //        var table = document.getElementById('kt_acciones_table');
    //        if (!table) {
    //            return;
    //        }

    //    var formRegistroAccion = document.getElementById('kt_modal_menu_acciones_form');
    //    if (!formRegistroAccion) {
    //        console.error('No se encontró el formulario de registro.');
    //        return;
    //    }
    //    var selectedData = getSelectedAcciones();
    //    $("#ListaAcciones").val(selectedData);
    //    // Validar si hay datos seleccionados
    //    if (selectedData.length === 0) {
    //        return;
    //    }
    //    var saveButton = document.getElementById('kt_save_estado_button');
    //    if (!saveButton) {
    //        console.error('No se encontró el botón de guardar.');
    //        return;
    //    }

    //    var validator = FormValidation.formValidation(
    //        formRegistroAccion,
    //        {
    //            fields: {
    //                'nIdMenu': {
    //                    validators: {
    //                        notEmpty: {
    //                            message: 'El menú es obligatorio'
    //                        }
    //                    }
    //                }
    //            },
    //            plugins: {
    //                trigger: new FormValidation.plugins.Trigger(),
    //                bootstrap: new FormValidation.plugins.Bootstrap5({
    //                    rowSelector: '.fv-row',
    //                    eleValidClass: '',
    //                    eleInvalidClass: '',
    //                })
    //            }
    //        }
    //    );

    //    saveButton.addEventListener('click', function (e) {
    //        e.preventDefault();

    //        // Validar el formulario
    //        validator.validate().then(function (status) {
    //            if (status == 'Valid') {
    //                // Obtener datos seleccionados
    //                var selectedData = getSelectedAcciones();
    //                $("#ListaAcciones").val(selectedData);
    //                // Validar si hay datos seleccionados
    //                if (selectedData.length === 0) {
    //                    Swal.fire({
    //                        text: 'No hay datos seleccionados para guardar.',
    //                        icon: 'warning',
    //                        buttonsStyling: false,
    //                        confirmButtonText: 'Ok',
    //                        customClass: {
    //                            confirmButton: 'btn fw-bold btn-primary',
    //                        }
    //                    });
    //                    return;
    //                }
    //                //$('#nIdMenuAccion').val(nIdMenu);
    //                //$('#nIdRolAccion').val(nIdRol);
    //                // Preparar el objeto de solicitud
    //                var request = {
    //                    nIdMenu: $("#nIdMenuAccion").val(),
    //                    nIdRol: $("#nIdRolAccion").val(),
    //                    ListaAcciones: selectedData
    //                };

    //                // Activar indicador de carga
    //                saveButton.setAttribute('data-kt-indicator', 'on');
    //                saveButton.disabled = true;

    //                // Enviar solicitud AJAX
    //                $.ajax({
    //                    type: 'POST',
    //                    url: globalPath + 'Perfil/RegistrarPerfilAccion',
    //                    contentType: 'application/json',
    //                    data: JSON.stringify(request),
    //                    success: function (data) {
    //                        saveButton.removeAttribute('data-kt-indicator');
    //                        saveButton.disabled = false;

    //                        if (data.succeeded) {
    //                            Swal.fire({
    //                                text: data.message,
    //                                icon: 'success',
    //                                buttonsStyling: false,
    //                                confirmButtonText: 'Listo',
    //                                customClass: {
    //                                    confirmButton: 'btn fw-bold btn-primary',
    //                                }
    //                            }).then(function () {
    //                                $(window).attr('location', globalPath + 'Perfil');
    //                            });
    //                        } else {
    //                            messageError(data.message);
    //                        }
    //                    },
    //                    error: function (jqXHR, textStatus, errorThrown) {
    //                        saveButton.removeAttribute('data-kt-indicator');
    //                        saveButton.disabled = false;
    //                        messageError(errorThrown);
    //                    }
    //                });
    //            }
    //        });
    //    });
    //};

    //var handleModalControlAcciones = function () {
    //    var table = document.getElementById('kt_acciones_table');
    //    if (!table) {
    //        return;
    //    }

    //    var form = document.getElementById('kt_modal_menu_acciones_form');
    //    if (!form) {
    //        return;
    //    }
    //    if ($("#cIdAccion").val().length === 0) {
    //        return;
    //    }
    //    var saveButton = $('#kt_save_estado_button');
    //    var validator;

    //    validator = FormValidation.formValidation(
    //        form,
    //        {
    //            fields: {
    //                'nIdMenu': {
    //                    validators: {
    //                        notEmpty: {
    //                            message: 'Menú es obligatorio'
    //                        }
    //                    }
    //                },
    //            }
    //        });
    //    //RegistroOperacion.getRevalidateFormElement(form, 'nIdMenu', validator);
    //    //RegistroOperacion.getRevalidateFormElement(form, 'nIdRol', validator);
    //    saveButton.off('click');
    //    saveButton.on('click', function (e) {
    //        e.preventDefault();
    //        validator.validate().then(function (status) {
    //            if (status == 'Valid') {

    //                var pselectedData = $("#cIdAccion").val();
    //                if (pselectedData.length === 0) {
    //                    Swal.fire({
    //                        text: 'No hay datos seleccionados para guardar.',
    //                        icon: 'warning',
    //                        buttonsStyling: false,
    //                        confirmButtonText: 'Ok',
    //                        customClass: {
    //                            confirmButton: 'btn fw-bold btn-primary',
    //                        }
    //                    });
    //                    return;
    //                }
    //                saveButton.attr('data-kt-indicator', 'on');
    //                saveButton.prop('disabled', true);
    //                setTimeout(function () {
    //                    $.ajax({
    //                        type: 'POST',
    //                        dataType: 'json',
    //                        url: $(form).attr('action'),
    //                        xhrFields: {
    //                            withCredentials: true
    //                        },
    //                        data: $(form).serializeObject(),
    //                        success: function (data) {
    //                            console.log('datadatadata',data)
    //                            if (data.succeeded) {
    //                                Swal.fire({
    //                                    text: data.message,
    //                                    icon: 'success',
    //                                    buttonsStyling: false,
    //                                    confirmButtonText: 'Listo',
    //                                    customClass: {
    //                                        confirmButton: 'btn btn-primary'
    //                                    }
    //                                }).then(function (result) {
    //                                    if (result.isConfirmed) {
    //                                        $(window).attr('location', globalPath + 'Perfil');
    //                                    }
    //                                });
    //                            } else {
    //                                saveButton.removeAttr('data-kt-indicator');
    //                                saveButton.prop('disabled', false);
    //                                messageError(data.message);
    //                            }
    //                        },
    //                        error: function (jqXHR, textStatus, errorThrown) {
    //                            saveButton.removeAttr('data-kt-indicator');
    //                            saveButton.prop('disabled', false);
    //                            messageError(errorThrown);
    //                        }
    //                    });
    //                }, 2000);
    //            } else {
    //                messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
    //            }
    //        });
    //    });
    //}

    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            initDataTableRol();
            handleDeletePerfilForm();
            handleRegistroPerfil();
        },
        getRevalidateFormElement: function (form, elem, val) {
            handleRevalidateFormElement(form, elem, val);
        },
    }

}();

KTUtil.onDOMContentLoaded(function () {
    Perfil.init();
});