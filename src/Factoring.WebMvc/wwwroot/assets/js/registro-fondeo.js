'use strict';
var globalPath = $('base').attr('href');
var Fondeo = function () {
    var datatable;
    var initDatatable = function () {
        var table = document.getElementById('kt_fondeo_table');
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
                url: globalPath + 'Fondeo/ListadoRegistros',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdFondeadorFactura', name: 'nIdFondeador', 'autoWidth': true, class: 'text-center' },
                { data: 'cNroOperacion', 'autoWidth': true, class: 'text-center' },
                { data: 'cGirador', 'autoWidth': true, class: 'text-center' },
                { data: 'cNumeroAsignacion', 'autoWidth': true, class: 'text-left' },
                { data: 'cFondeadorAsignado', 'autoWidth': true, class: 'text-left' },
                { data: 'nMontoADesembolsarFondeador', 'autoWidth': true, class: 'text-left' },
                { data: 'cEstadoFondeo', 'autoWidth': true, class: 'text-left' },
                { data: 'cMoneda', 'autoWidth': true, class: 'text-left' },
                { data: null, 'autoWidth': true, class: 'text-center', responsivePriority: -1 }
            ],
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    className: 'text-center',
                    render: function (data, type, row) {
                        var checkbox = `<div class="form-check form-check-sm form-check-custom form-check-solid"><input class="form-check-input checkbox-main" type="checkbox" value="${row.iIdFondeador}" /></div>`;
                        return (row.iEstadoFile == 1) ? `` : checkbox;
                    }
                },
                {
                    //targets: -1,
                    //data: null,
                    //orderable: false,
                    //className: 'text-center',
                    //render: function (data, type, row) {
                    //    var button = `<button type="button" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row"><i class="las la-pen fs-2"></i></button>`;
                    //    return (data.iEstadoFile == 1) ? `` : button;
                    //}
                    targets: -1,
                    data: null,
                    orderable: false,
                    className: 'text-end',
                    render: function (data, type, row) {
                        //return `<a href="javascript:;" class="btn btn-outline btn-outline-solid btn-outline-primary btn-active-light-primary btn-icon-primary btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end" data-kt-menu-flip="top-end">
                        //    Acciones
                        //    <span class="svg-icon svg-icon-5 m-0">
                        //        <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">
                        //            <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                        //                <polygon points="0 0 24 0 24 24 0 24"></polygon>
                        //                <path d="M6.70710678,15.7071068 C6.31658249,16.0976311 5.68341751,16.0976311 5.29289322,15.7071068 C4.90236893,15.3165825 4.90236893,14.6834175 5.29289322,14.2928932 L11.2928932,8.29289322 C11.6714722,7.91431428 12.2810586,7.90106866 12.6757246,8.26284586 L18.6757246,13.7628459 C19.0828436,14.1360383 19.1103465,14.7686056 18.7371541,15.1757246 C18.3639617,15.5828436 17.7313944,15.6103465 17.3242754,15.2371541 L12.0300757,10.3841378 L6.70710678,15.7071068 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000003, 11.999999) rotate(-180.000000) translate(-12.000003, -11.999999)"></path>
                        //            </g>
                        //         </svg>
                        //    </span>
                        //</a>
                        //<div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-bold fs-7 w-125px py-4" data-kt-menu="true">
                        //    <div class="menu-item px-3">
                        //        <a href="` + globalPath + `Fondeador/Registro?fondeadorId=` + data.nIdFondeador + `" class="menu-link px-3">Editar</a>
                        //    </div>

                        //</div>`;


                        var buttonAction = ``;
                        console.log(data)
                        if (data.nEstadoFondeo == 5 && data.nIdFondeador != 0) { //ANULADO Y CON FONDEADOR ASIGNADO
                            buttonAction = `<a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="ver" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Ver"><i class="las la-search fs-2"></i></a> 
                              <button data-add-table="add_row" data-row= ${data.nIdFondeadorFactura}  class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-eli" title="Agregar"><i class="las la-plus fs-2"></i></button> `
                        } else if (data.nEstadoFondeo == 1) {//PENDIENTE ASIGNAR
                            buttonAction += `

                         <button data-delete-table="delete_row" data-row= ${data.nIdFondeadorFactura}  class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-eli" title="Anular"><i class="las la-ban fs-2 text-danger"></i></button> 

                      

                        <a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="editar" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Editar"><i class="las la-pen fs-2"></i></a>

                                

                                `;
                        } else if (data.nEstadoFondeo == 5) {//ANULADO
                            buttonAction = `<a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="ver" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Ver"><i class="las la-search fs-2"></i></a> 
                             `

                        }
                        else if (data.nEstadoFondeo == 3 ) {//DESEMBOLSADO
                            buttonAction += `

                         

                      

                        <a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="editar" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Editar"><i class="las la-pen fs-2"></i></a>

                                

                                <a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="ver" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Ver"><i class="las la-search fs-2"></i></a> 


                                <button data-add-table="add_row" data-row= ${data.nIdFondeadorFactura}  class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-eli" title="Agregar"><i class="las la-plus fs-2"></i></button> 

                                `;

                        } else {
                            buttonAction += `

                         <button data-delete-table="delete_row" data-row= ${data.nIdFondeadorFactura}  class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-eli" title="Anular"><i class="las la-ban fs-2 text-danger"></i></button> 

                      

                        <a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="editar" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Editar"><i class="las la-pen fs-2"></i></a>

                                

                                <a href="javascript:;" class="btn btn-sm btn-icon btn-light-dark open-modal edit-row me-2 p-act" data-bs-toggle="modal" data-bs-target="#kt_modal_registro_datos_fondeo" data-n-accion="ver" data-n-operacion=${data.nIdOperaciones} data-n-tipo-fondeo=${data.nIdTipoFondeo} data-n-fondeador-factura=${data.nIdFondeadorFactura} data-n-data=${JSON.stringify(data).replace(/\s+/g, "")} title="Ver"><i class="las la-search fs-2"></i></a> 


                                

                                `;
                        }
                        
                        //<a href="${globalPath}Operacion/Registro?operacionId=${data.nIdOperaciones}" class="btn btn-sm btn-icon btn-light btn-active-light-primary edit-row me-2 p-act" title="Editar"><i class="las la-pen fs-2"></i></a>
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
            $(searchClear).show();
            KTMenu.createInstances();
            handleDeleteFondeoForm();
            handleAddFondeoForm();
            //initToggleToolbar();
            //toggleToolbars();
            //handleModalControlDocumentario();
        });
    }

    var handleFilterTable = function () {

        $('#FechaDesembolso').flatpickr({
            dateFormat: 'd/m/Y'
            /*,defaultDate: 'today'*/
        });
        $('#FechaCobranza').flatpickr({
            dateFormat: 'd/m/Y'
            /*,defaultDate: 'today'*/
        });
        $('#FechaRegistro').flatpickr({
            dateFormat: 'd/m/Y'
            ,defaultDate: 'today'
        });

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
            $("#IdEstadoFondeo").val('').trigger('change');
            initDatatable();
        });


    }

    var handleModalRegistroDatos = function () {
        var form = document.getElementById('kt_modal_registro_fondeo_form');
        if (!form) {
            return;
        }

        var saveButton = document.getElementById('kt_save_fondeo_button');
        var validator;

        //validator = FormValidation.formValidation(
        //    form,
        //    {
        //        fields: {
        //            'nIdEstadoEvaluacion': {
        //                validators: {
        //                    notEmpty: {
        //                        message: 'Estado es obligatorio'
        //                    }
        //                }
        //            },
        //        },
        //        plugins: {
        //            trigger: new FormValidation.plugins.Trigger(),
        //            bootstrap: new FormValidation.plugins.Bootstrap5({
        //                rowSelector: '.fv-row',
        //                eleValidClass: '',
        //                eleInvalidClass: '',
        //            })
        //        }
        //    });
        //Fondeo.getRevalidateFormElement(form, 'nIdEstadoEvaluacion', validator);
        //Fondeo.getRevalidateFormElement(form, 'cComentario', validator);
        saveButton.addEventListener('click', function (e) {

            e.preventDefault();

            var tipoProducto = $('#IdFondeador').find('option:selected').data('tipo-producto');
            if (tipoProducto === undefined) {
                messageError('Por favor, seleccione un fondeador y vuelve a intentarlo.');
                return false;
            }
            console.log(tipoProducto)
            if (tipoProducto == 2) {
                var PorTasaMensual = $('#PorTasaMensual').val().trim();
                var PorComisionFactura = $('#PorComisionFactura').val().trim();;
                var PorSpread = $('#PorSpread').val().trim();;
                console.log(PorTasaMensual, PorComisionFactura, PorSpread)

                if (PorTasaMensual == '' || PorComisionFactura == '' || PorSpread == '') {
                    messageError('Por favor, ingrese los campos requeridos y vuelve a intentarlo.');
                    return false;
                }
            } else if (tipoProducto == 1) {
                var PorCapitalFinanciado = $('#PorCapitalFinanciado').val();
                var PorTasaAnualFondeo = $('#PorTasaAnualFondeo').val();
                //var PorTasaMoraFondeo = $('#PorTasaMoraFondeo').val();
                //console.log(PorCapitalFinanciado, PorTasaAnualFondeo, PorTasaMoraFondeo)

                /*  if (PorCapitalFinanciado == '' || PorTasaAnualFondeo == '' || PorTasaMoraFondeo == '') {*/
                if (PorCapitalFinanciado == '' || PorTasaAnualFondeo == '') {
                    messageError('Por favor, ingrese los campos requeridos y vuelve a intentarlo.');
                    return false;
                }

            }
            //validator.validate().then(function (status) {
            //    if (status == 'Valid') {
                    saveButton.setAttribute('data-kt-indicator', 'on');
                    saveButton.disabled = true;
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
                                            //$(window).attr('location', globalPath + 'Operacion');
                                            initDatatable();
                                            saveButton.removeAttribute('data-kt-indicator');
                                            saveButton.disabled = false;
                                            //$('#kt_modal_registro_datos_fondeo').modal('hide');
                                        }
                                    });
                                } else {
                                    saveButton.removeAttribute('data-kt-indicator');
                                    saveButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttribute('data-kt-indicator');
                                saveButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });
                    }, 2000);
            //    } else {
            //        messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
            //    }
            //});
        });
    }
    var handleDeleteFondeoForm = function () {
        var deleteRow = document.querySelectorAll('[data-delete-table="delete_row"]');
        if (!deleteRow) {
            return;
        }
        deleteRow.forEach(d => {
            d.addEventListener('click', function (e) {
                
                e.preventDefault();
                var idFondeadorFactura = $(this).data('row');
                console.log(idFondeadorFactura)
                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de anular el fondeo?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, anular!',
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
                            url: globalPath + 'Fondeo/ActualizaEstadoFondeo',
                            data: {
                                IdFondeadorFactura: idFondeadorFactura
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                debugger;
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: 'Anulaste correctamente el fondeo.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        //datatable.row($(parent)).remove().draw();
                                        initDatatable();
                                    });
                                } else {
                                    messageError('El fondeo no fue anulado (' + data.message + ')');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                       /* messageError('El fondeo no fue anulada.');*/
                    }
                });
            });
        });
    }

    var handleAddFondeoForm = function () {
        var deleteRow = document.querySelectorAll('[data-add-table="add_row"]');
        if (!deleteRow) {
            return;
        }
        deleteRow.forEach(d => {
            d.addEventListener('click', function (e) {

                e.preventDefault();
                var idFondeadorFactura = $(this).data('row');
                console.log(idFondeadorFactura)
                var parent = e.target.closest('tr');
                Swal.fire({
                    text: '¿Estás seguro de agregar un nuevo fondeo?',
                    icon: 'warning',
                    showCancelButton: true,
                    buttonsStyling: false,
                    confirmButtonText: 'Sí, agregar!',
                    cancelButtonText: 'Cancelar',
                    customClass: {
                        confirmButton: 'btn fw-bold btn-primary',
                        cancelButton: 'btn fw-bold btn-active-light-primary'
                    }
                }).then(function (result) {

                    if (result.value) {
                        var token = $('input[name="__RequestVerificationToken"]').val();
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: globalPath + 'Fondeo/AgregarFondeo',
                            data: {
                                IdFondeadorFactura: idFondeadorFactura
                            },
                            headers: {
                                'RequestVerificationToken': token
                            },
                            success: function (data) {
                                debugger;
                                if (data.succeeded) {
                                    Swal.fire({
                                        text: 'Agregaste correctamente el fondeo.',
                                        icon: 'success',
                                        buttonsStyling: false,
                                        confirmButtonText: 'Listo',
                                        customClass: {
                                            confirmButton: 'btn fw-bold btn-primary',
                                        }
                                    }).then(function () {
                                        //datatable.row($(parent)).remove().draw();
                                        initDatatable();
                                    });
                                } else {
                                    messageError('El fondeo no fue agregado (' + data.message + ')');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                messageError(errorThrown);
                            }
                        });
                    } else if (result.dismiss === 'cancel') {
                        /* messageError('El fondeo no fue anulada.');*/
                    }
                });
            });
        });
    }

    $('#kt_modal_registro_datos_fondeo').on('show.bs.modal', function (event) {
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };

        var button = $(event.relatedTarget); // Botón que activó el modal
        var nAccion = button.data('n-accion');
        var flg = true;
        if (nAccion == 'ver') {
            $('#kt_save_fondeo_button').hide();
        } else if (nAccion == 'editar') {
            $('#kt_save_fondeo_button').show();
            flg = false;
        }

        $('#PorCapitalFinanciado').attr('readonly', flg);
        $('#PorTasaAnualFondeo').attr('readonly', flg);
        //$('#PorTasaMoraFondeo').attr('readonly', flg);
        $('#PorTasaMensual').attr('readonly', flg);
        $('#PorComisionFactura').attr('readonly', flg);
        $('#PorSpread').attr('readonly', flg);
        $('#FechaDesembolso').attr('readonly', flg);
        $('#FechaCobranza').attr('readonly', flg);
        $('#FechaDesembolso').attr('readonly', flg);
        $('#FechaCobranza').attr('readonly', flg);

        $('#IdFondeador').prop('disabled', flg);
        $('#FechaDesembolso').prop('disabled', flg);
        $('#FechaCobranza').prop('disabled', flg);

      


        var nOpe = button.data('n-operacion');
        var nTipoFondeo = button.data('n-tipo-fondeo'); 
        var nFondeadorFactura = button.data('n-fondeador-factura');

        var nData = button.data('n-data');
        console.log('Valor data-n-data:', nData);

        console.log('Valor data-n-nOpe:', nOpe);
        console.log('Valor data-n-tipo-fondeo:', nTipoFondeo);

        $.ajax({
            url: globalPath + 'Fondeador/ListadoRegistrosByTipoFondeo',  // Cambia esto por la URL de tu API o servidor
            data: {
                tipoFondeo: nTipoFondeo
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                // Limpiar el select
                console.log(response)
                $('#IdFondeador').empty();

                // Recorrer los datos recibidos y añadir opciones al select
                $.each(response.data, function (index, item) {
                    $('#IdFondeador').append($('<option>', {
                        value: item.nIdFondeador,  
                        text: item.cNombreFondeador,
                        'data-tipo-producto': item.nIdProducto
                    }));
                });

                $('#IdFondeador').val(nData.nIdFondeador).trigger('change');

                //$('#PorCapitalFinanciado').val(nData.nPorcentajeCapitalFinanciado == 0 ? '' : nData.nPorcentajeCapitalFinanciado)
                //$('#PorTasaAnualFondeo').val(nData.nPorcentajeTasaAnualFondeo == 0 ? '' : nData.nPorcentajeTasaAnualFondeo)
                //$('#PorTasaMoraFondeo').val(nData.nPorcentajeTasaMoraFondeo == 0 ? '' : nData.nPorcentajeTasaMoraFondeo)
                //$('#PorTasaMensual').val(nData.nPorcentajeTasaMensual == 0 ? '' : nData.nPorcentajeTasaMensual)
                //$('#PorComisionFactura').val(nData.nPorcentajeComisionFactura == 0 ? '' : nData.nPorcentajeComisionFactura)
                //$('#PorSpread').val(nData.nPorcentajeSpread == 0 ? '' : nData.nPorcentajeSpread)


                //$('#FechaDesembolso').val(nData.dFechaDesembolsoFondeador == 0 ? '' : nData.dFechaDesembolsoFondeador)

                $('#PorCapitalFinanciado').val(nData.nPorcentajeCapitalFinanciado)
                $('#PorTasaAnualFondeo').val(nData.nPorcentajeTasaAnualFondeo)
                //$('#PorTasaMoraFondeo').val(nData.nPorcentajeTasaMoraFondeo)
                $('#PorTasaMensual').val(nData.nPorcentajeTasaMensual)
                $('#PorComisionFactura').val(nData.nPorcentajeComisionFactura)
                $('#PorSpread').val(nData.nPorcentajeSpread)


                $('#FechaDesembolso').val(nData.dFechaDesembolsoFondeador)
                console.log('Fechadesembolso',nData.dFechaDesembolsoFondeador)
                if ( nData.dFechaDesembolsoFondeador != null ) {
                    $('#FechaDesembolso').prop('disabled', true);
                }

            },
            error: function () {
                alert('Hubo un error al cargar los fondeadores.');
            }
        });


        $('#nIdEstadoEvaluacion').select2({
            allowClear: true,
            dropdownParent: $('#kt_modal_registro_datos_fondeo')
        });


        $('#IdOperacion').val(nOpe);
        $('#IdTipoProducto').val(nTipoFondeo);
        $('#IdFondeadorFactura').val(nFondeadorFactura);


    });

    $('#IdFondeador').on('change', function () {
        var selectedOption = $(this).find('option:selected');
        var tipoProducto = selectedOption.data('tipo-producto');
        $('#IdFondeadorVal').val($('#IdFondeador').val());
        console.log('Tipo de Producto seleccionado:', tipoProducto);
        if (tipoProducto !== undefined) {
            if (tipoProducto == 2) {
                $('#sec-cobranzaLibre').show();
                $('#sec-factoring').hide();
            } else if (tipoProducto == 1) {
                $('#sec-factoring').show();
                $('#sec-cobranzaLibre').hide();
            } else {
                $('#sec-factoring').hide();
                $('#sec-cobranzaLibre').hide();
            }
        } else {
            $('#sec-factoring').hide();
            $('#sec-cobranzaLibre').hide();
        }

    });
    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            handleModalRegistroDatos();
            handleDeleteFondeoForm();
            handleAddFondeoForm();
           
        }
    }



}();

KTUtil.onDOMContentLoaded(function () {
    Fondeo.init();
});