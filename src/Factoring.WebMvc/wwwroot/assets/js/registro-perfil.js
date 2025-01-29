'use strict';
var globalPath = $('base').attr('href');
var Perfil = function () {
    var datatable;
    var datatableRol;
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
    var initDataTableRol = function () {
        var nIdRol = document.getElementById('nIdRol');
        const element = document.getElementById("aBuscarGrilla");
        const hbuscar = element ? element.value : null;

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
                {
                    data: 'cInsertar',
                    autoWidth: true,
                    class: 'text-center',
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="checkbox-create" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                    }
                },
                {
                    data: 'cActualizar',
                    autoWidth: true,
                    class: 'text-center',
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="checkbox-update" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                    }
                },
                {
                    data: 'cConsultar',
                    autoWidth: true,
                    class: 'text-center',
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="checkbox-read" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                    }
                },
                {
                    data: 'cEliminar',
                    autoWidth: true,
                    class: 'text-center',
                    render: function (data, type, row) {
                        return `<input type="checkbox" class="checkbox-delete" data-id="${row.nIdMenuDetalle}" data-menu="${row.nIdMenu}" ${data == 1 ? 'checked' : ''}>`;
                    }
                }
            ]
        });
        datatableRol.on('draw', function () {
            Common.init();
            if (hbuscar == "3") {
                document.querySelectorAll("#kt_menu_table input[type='checkbox']").forEach(element => {
                    element.disabled = true;
                });
            }
        });
    };
    var getSelectedMenuDetails = function () {
        var selectedData = [];
        $('#kt_menu_table tbody tr').each(function () {
            var nIdMenuDetalle = +$(this).find('td:first').text();
            var nIdMenu = +$(this).find('.checkbox-create, .checkbox-update, .checkbox-read, .checkbox-delete').data('menu');
            if ($(this).find('.checkbox-create:checked').length > 0 ||
                $(this).find('.checkbox-update:checked').length > 0 ||
                $(this).find('.checkbox-read:checked').length > 0 ||
                $(this).find('.checkbox-delete:checked').length > 0) {
                selectedData.push({
                    nIdMenuDetalle: nIdMenuDetalle,
                    nIdMenu: nIdMenu,
                    Insertar: $(this).find('.checkbox-create:checked').length > 0,
                    Actualizar: $(this).find('.checkbox-update:checked').length > 0,
                    Consultar: $(this).find('.checkbox-read:checked').length > 0,
                    Eliminar: $(this).find('.checkbox-delete:checked').length > 0
                });
            }
        });
        return selectedData;
    };
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
                    // Obtener datos seleccionados
                    var selectedData = getSelectedMenuDetails();

                    // Validar si hay datos seleccionados
                    if (selectedData.length === 0) {
                        Swal.fire({
                            text: 'No hay datos seleccionados para guardar.',
                            icon: 'warning',
                            buttonsStyling: false,
                            confirmButtonText: 'Ok',
                            customClass: {
                                confirmButton: 'btn fw-bold btn-primary',
                            }
                        });
                        return;
                    }

                    // Preparar el objeto de solicitud
                    var request = {
                        cNombreRol: $("#cNombreRol").val(),
                        ListaMenu: selectedData
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
                                    $(window).attr('location', globalPath + 'Perfil');
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

    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            initDataTableRol();
            handleDeletePerfilForm();
            handleRegistroPerfil();
        }
    }

}();

KTUtil.onDOMContentLoaded(function () {
    Perfil.init();
});