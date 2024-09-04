'use strict';
var globalPath = $('base').attr('href');
var Transferencias = function () {
    var datatable;

    $('#InversionistaAccion').on('change', function () {
        const selectedValue = $(this).val();
        toggleToolbarsValidarNew();
      
         if (selectedValue == "2") {
            $('#dvInversionista').show();
            $('#dvMotivo').hide();
        }
         else if (selectedValue == "3") {
             $('#dvMotivo').show();
             $('#dvInversionista').hide();  
         }
        else {
            $('#dvInversionista').hide();
            $('#dvMotivo').hide();
         }

        if (selectedValue) {
            console.log("Valor seleccionado: " + selectedValue);
            $('#nValorSeleccionado').val(selectedValue);
        } else {
            console.log("No se ha seleccionado ningún valor.");
        }
    });

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
            $(this).closest('form').find('select').val('').trigger('change');
            initDatatable();
        });
    }
    var initDatatable = function () {
        var table = document.getElementById('kt_transferencias_table');
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
                url: globalPath + 'Transferencias/GetOperacionAllList',
                type: 'POST',
                datatype: 'json',
                data: $('#kt_search_form').serializeObject()
            },
            columns: [
                { data: 'nIdOperacionesFacturas', 'Width': '10%', class: 'text-left' },
                { data: 'nNroOperacion', 'Width': '10%', class: 'text-center' },
                { data: 'cNroDocumento', 'Width': '20%', class: 'text-left' },
                /*{ data: 'numero', 'Width': '20%', class: 'text-left' },*/
                { data: 'nMonto', 'Width': '15%', class: 'text-end' },
                {
                    data: 'dFechaPagoNegociado', 'Width': '10%', class: 'text-center', render: function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                { data: 'cProcessResult', 'Width': '100%', class: '' },
                { data: 'estado', 'Width': '15%', class: 'text-center' }
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row) {
                        console.log(row);

                        return `<div class="form-check form-check-sm form-check-custom form-check-solid"><input class="form-check-input checkbox-main" type="checkbox" data-estado="${row.estadoId}" value="${data}" /></div>`;
                    }
                }
            ]
        });
        table = datatable.$;
        datatable.on('draw', function () {
            initToggleToolbar();
            toggleToolbars();
            var searchButton = document.getElementById('kt_search_button');
            var searchClear = document.getElementById('kt_search_clear');
            searchButton.removeAttribute('data-kt-indicator');
            searchButton.disabled = false;
            $(searchClear).show();
            KTMenu.createInstances();
        });
    }
    var initToggleToolbar = function () {
        $('#btn_transferir, #btn_traspaso').hide();
        var container = document.querySelector('#kt_transferencias_table');
        var checkboxes = container.querySelectorAll('[type="checkbox"]');
        //var registerSelected = document.querySelector('[data-kt-transferencias-table-select="register_selected"]');
        //var transferSelected = document.querySelector('[data-kt-transferencias-table-select="transfer_selected"]');
        //var traspasarSelected = document.querySelector('[data-kt-transferencias-table-select="traspasar_selected"]');
        //var removerSelected = document.querySelector('[data-kt-transferencias-table-select="remover_selected"]');
        var accionSelected = document.querySelector('[data-kt-transferencias-table-select="accion_selected"]');
        checkboxes.forEach(c => {
            c.addEventListener('click', function () {
                setTimeout(function () {
                    toggleToolbars();
                }, 50);
            });
        });


        accionSelected.addEventListener('click', function () {
            Swal.fire({
                text: '¿Está seguro que quiere continuar con el proceso de Registro y Anotación para las facturas seleccionadas?',
                icon: 'warning',
                showCancelButton: true,
                buttonsStyling: false,
                showLoaderOnConfirm: true,
                confirmButtonText: 'Sí, continuar',
                cancelButtonText: 'Cancelar',
                customClass: {
                    confirmButton: 'btn fw-bold btn-danger',
                    cancelButton: 'btn fw-bold btn-active-light-primary'
                },
            }).then(function (result) {
                if (result.value) {
                    var IdFacturas = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
                        return +e.value;
                    });
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: globalPath + 'Transferencias/Registro',
                        data: {
                            IdFacturas: IdFacturas
                        },
                        success: function (data) {

                            if (typeof data.error != 'undefined') {
                                if (data.error === false) {
                                    if (data.valores.body.processId !== 0) {
                                        Swal.fire({
                                            text: data.valores.body.message,
                                            icon: 'success',
                                            buttonsStyling: false,
                                            confirmButtonText: 'Listo',
                                            customClass: {
                                                confirmButton: 'btn fw-bold btn-primary',
                                            }
                                        }).then(function () {
                                            $(window).attr('location', globalPath + 'Transferencias');
                                        });
                                    } else {
                                        //saveButton.removeAttribute('data-kt-indicator');
                                        //saveButton.disabled = false;
                                        messageError(data.mensaje);
                                    }
                                }
                                else {
                                    //saveButton.removeAttribute('data-kt-indicator');
                                    //saveButton.disabled = false;
                                    messageError(data.mensaje);
                                }
                            }
                            else {
                                messageError(data);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            messageError(errorThrown);
                        }
                    });
                }
            });
        });



        //transferSelected.addEventListener('click', function () {
        //    Swal.fire({
        //        text: '¿Está seguro que quiere continuar con el proceso de Transferencia para las facturas seleccionadas?',
        //        icon: 'warning',
        //        showCancelButton: true,
        //        buttonsStyling: false,
        //        showLoaderOnConfirm: true,
        //        confirmButtonText: 'Sí, continuar',
        //        cancelButtonText: 'Cancelar',
        //        customClass: {
        //            confirmButton: 'btn fw-bold btn-danger',
        //            cancelButton: 'btn fw-bold btn-active-light-primary'
        //        },
        //    }).then(function (result) {
        //        if (result.value) {
        //            var IdFacturas = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
        //                return +e.value;
        //            });
        //            $('#kt_modal_transferencia').find('input:hidden[name="IdFacturas[]"]').val(IdFacturas);
        //            $('#kt_modal_transferencia').modal('show');
        //        }
        //    });
        //});
        //traspasarSelected.addEventListener('click', function () {
        //    Swal.fire({
        //        text: '¿Está seguro que quiere continuar con el proceso de traspaso para las facturas seleccionadas?',
        //        icon: 'warning',
        //        showCancelButton: true,
        //        buttonsStyling: false,
        //        showLoaderOnConfirm: true,
        //        confirmButtonText: 'Sí, continuar',
        //        cancelButtonText: 'Cancelar',
        //        customClass: {
        //            confirmButton: 'btn fw-bold btn-danger',
        //            cancelButton: 'btn fw-bold btn-active-light-primary'
        //        },
        //    }).then(function (result) {
        //        if (result.value) {
        //            var IdFacturas = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
        //                return +e.value;
        //            });
        //            $('#kt_modal_traspasar').find('input:hidden[name="IdFacturasTraspaso[]"]').val(IdFacturas);
        //            $('#kt_modal_traspasar').modal('show');
        //        }
        //    });
        //});
        //removerSelected.addEventListener('click', function () {
        //    Swal.fire({
        //        text: '¿Está seguro que quiere continuar con el proceso de remover para las facturas seleccionadas?',
        //        icon: 'warning',
        //        showCancelButton: true,
        //        buttonsStyling: false,
        //        showLoaderOnConfirm: true,
        //        confirmButtonText: 'Sí, continuar',
        //        cancelButtonText: 'Cancelar',
        //        customClass: {
        //            confirmButton: 'btn fw-bold btn-danger',
        //            cancelButton: 'btn fw-bold btn-active-light-primary'
        //        },
        //    }).then(function (result) {
        //        if (result.value) {
        //            var IdFacturas = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
        //                return +e.value;
        //            });
        //            $('#kt_modal_remover').find('input:hidden[name="IdFacturasRemover[]"]').val(IdFacturas);
        //            $('#kt_modal_remover').modal('show');
        //        }
        //    });
        //});
        accionSelected.addEventListener('click', function () {
            Swal.fire({
                text: '¿Está seguro que quiere continuar con el proceso de envío a registrar a CAVALI las facturas seleccionadas?',
                icon: 'warning',
                showCancelButton: true,
                buttonsStyling: false,
                showLoaderOnConfirm: true,
                confirmButtonText: 'Sí, continuar',
                cancelButtonText: 'Cancelar',
                customClass: {
                    confirmButton: 'btn fw-bold btn-danger',
                    cancelButton: 'btn fw-bold btn-active-light-primary'
                },
            }).then(function (result) {
                if (result.value) {
                    var IdFacturas = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
                        return +e.value;
                    });
                    $('#kt_modal_accion').find('input:hidden[name="IdFacturasAccion[]"]').val(IdFacturas);
                    $('#kt_modal_accion').modal('show');
                }
            });
        });


    }
    var toggleToolbars = function () {
        var container = document.querySelector('#kt_transferencias_table');
        var toolbarSelected = document.querySelector('[data-kt-transferencias-table-toolbar="selected"]');
        var selectedCount = document.querySelector('[data-kt-transferencias-table-select="selected_count"]');
        var allCheckboxes = container.querySelectorAll('tbody [type="checkbox"]');
        var checkedState = false;
        //var pnIdOpcionOperacion = $("#InversionistaAccion").val();
        var count = 0;
        var countnew = selectedCount.innerHTML;
        $('#btn_traspaso').hide();
        $('#btn_transferir').hide();
        allCheckboxes.forEach(c => {
            if (c.checked) {
                checkedState = true;
                count++;
            }
            //else {
            //    if (countnew > 0) {
            //        countnew--;
            //    }
            //    count = countnew;
            //    selectedCount.innerHTML = count;
            //    if (count == 0) {
            //        toolbarSelected.classList.add('d-none');
            //    }
            //}
        });
        var IdFacturas = $.map($('input.checkbox-main:checkbox:checked'), function (e, i) {
            return +e.value;
        });
        $('#kt_modal_accion').find('input:hidden[name="IdFacturasAccion[]"]').val(IdFacturas);
        if (checkedState) {
                selectedCount.innerHTML = count;
                toolbarSelected.classList.remove('d-none');
        }
        //if (checkedState) {
        //    setTimeout(function () {
        //        $.ajax({
        //            type: 'POST',
        //            dataType: 'json',
        //            url: globalPath + 'Transferencias/ObtenerAsignaciones',
        //            xhrFields: {
        //                withCredentials: true
        //            },
        //            data: {
        //                IdFacturas: IdFacturas,
        //                nIdOpcionOperacion: pnIdOpcionOperacion
        //            },
        //            success: function (data) {
        //                if (typeof data.data != 'undefined') {
        //                    console.log(data);
        //                    if (typeof data.data.listaFondeador[0].cantidadInversionistas > 1) {
        //                        //saveButton.removeAttribute('data-kt-indicator');
        //                        //saveButton.disabled = false;
        //                        messageError('Las facturas seleccionadas tienen mas de UN INVERSIONISTA asociado.');
        //                    }
        //                    else {
        //                        if (typeof data.data.listaFondeador[0].iIdFondeador != 'undefined') {
        //                            $('#InversionistaTraspaso').empty().change();
        //                            $('#Inversionista').empty().change();
        //                            //if (data.data[0].traspaso == 1) {
        //                            //    $('#btn_traspaso').show();
        //                            //    $.each(data.data, function () {
        //                            //        $('#InversionistaTraspaso').append($("<option />").val(this.iIdFondeador).text(this.cNombreFondeador));
        //                            //    });
        //                            //} else {
        //                            //    $('#btn_traspaso').hide();
        //                            //}

        //                            if (data.data.listaFondeador[0].transferencia == 1) {
        //                                $('#btn_transferir').show();
        //                                $.each(data.data, function () {
        //                                    $('#Inversionista').append($("<option />").val(this.iIdFondeador).text(this.cNombreFondeador));
        //                                });
        //                            } else {
        //                                $('#btn_transferir').hide();
        //                            }


        //                        }
        //                        else {
        //                            //saveButton.removeAttribute('data-kt-indicator');
        //                            //saveButton.disabled = false;
        //                            messageError(data);
        //                        }
        //                    }
        //                }
        //                else {
        //                    $('#btn_traspaso').hide();
        //                    $('#btn_transferir').hide();
        //                    $('#Inversionista').empty().trigger("change");
        //                    //saveButton.removeAttribute('data-kt-indicator');                            
        //                    //saveButton.disabled = false;
        //                    messageError(data);
        //                }
        //            },
        //            error: function (jqXHR, textStatus, errorThrown) {
        //                //saveButton.removeAttribute('data-kt-indicator');
        //                //saveButton.disabled = false;
        //                messageError(errorThrown);
        //            }
        //        });
        //    },
        //   2000);
        //    selectedCount.innerHTML = count;
        //    toolbarSelected.classList.remove('d-none');
        //} else {
        //    toolbarSelected.classList.add('d-none');
        //}
    }




    //var toggleToolbarsValidar = function () {
    //    if ($("#IdFacturasAccion").val().length > 0) {
    //        var pnIdOpcionOperacion = $("#InversionistaAccion").val();
    //        setTimeout(function () {
    //            $.ajax({
    //                type: 'POST',
    //                dataType: 'json',
    //                url: globalPath + 'Transferencias/ObtenerAsignaciones',
    //                xhrFields: {
    //                    withCredentials: true
    //                },
    //                data: {
    //                    IdFacturas: $("#IdFacturasAccion").val(),
    //                    nIdOpcionOperacion: pnIdOpcionOperacion
    //                },
    //                success: function (data) {
    //                    console.log('looo::',data);
    //                    if (typeof data.data.listaFondeador != 'undefined') {
    //                        console.log(data);
    //                        if (typeof data.data.listaFondeador[0].cantidadInversionistas > 1) {
    //                            messageError('Las facturas seleccionadas tienen mas de UN INVERSIONISTA asociado.');
    //                        }
    //                        else {
    //                            if (typeof data.data.listaFondeador[0].iIdFondeador != 'undefined') {
    //                                $('#InversionistaTraspaso').empty().change();
    //                                $('#Inversionista').empty().change();
    //                                if (data.data.listaFondeador[0].transferencia == 1) {
    //                                   /* $('#btn_transferir').show();*/
    //                                    $.each(data.data, function () {
    //                                        $('#Inversionista').append($("<option />").val(this.iIdFondeador).text(this.cNombreFondeador));
    //                                    });
    //                                }
    //                                //else {
    //                                //    $('#btn_transferir').hide();
    //                                //}
    //                            }
    //                            else {
    //                                messageError(data);
    //                            }
    //                        }
    //                    }
    //                    else {
    //                        $('#btn_traspaso').hide();
    //                        $('#btn_transferir').hide();
    //                        $('#Inversionista').empty().trigger("change");
    //                        messageError(data);
    //                    }
    //                },
    //                error: function (jqXHR, textStatus, errorThrown) {
    //                    messageError(errorThrown);
    //                }
    //            });
    //        }, 2000);
    //        //selectedCount.innerHTML = count;
    //        //toolbarSelected.classList.remove('d-none');
    //    }
    //    //else {
    //    //    toolbarSelected.classList.add('d-none');
    //    //}
    //}


    var toggleToolbarsValidarNew = function () {
        if ($("#IdFacturasAccion").val().length > 0) {
            var pnIdOpcionOperacion = $("#InversionistaAccion").val();
            var mensaje;
            if (pnIdOpcionOperacion == 1) {
                mensaje = 'Las facturas seleccionadas deben estar en estado Registrado.'
            }
            else if (pnIdOpcionOperacion == 2) {
                mensaje = 'Las facturas seleccionadas deben estar en estado Registrado.'
            }
            else {
                //3
                mensaje = 'Las facturas seleccionadas no deben estar en estado Desembolsado.'
            }
            setTimeout(function () {
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: globalPath + 'Transferencias/ObtenerAsignacionesCavali',
                    xhrFields: {
                        withCredentials: true
                    },
                    data: {
                        IdFacturas: $("#IdFacturasAccion").val(),
                        nIdOpcionOperacion: pnIdOpcionOperacion
                    },
                    success: function (data) {
                        //typeof data.listaFacturas != 'undefined' &&
                        console.log('data::', data);
                        console.log('data_lista::', data.listaFacturas);
                        if (data.data.listaFacturas.length > 0) {
                            console.log(data);
                            if (pnIdOpcionOperacion == 1 || pnIdOpcionOperacion == 3) {
                                if (data.data.nActivarTransferencia == 0 && data.data.nActivarTransferencia != null  ) {
                                    messageError(mensaje);
                                }
                            }
                            else {

                                if (data.data.listaFacturas[0].nCantOperacion > 1 ) {
                                    messageError('Las facturas seleccionadas se encuentran en diferente Nro de operación');
                                }
                                else if (data.data.listaFacturas[0].nCantOperacion = 0) {
                                    messageError('Las facturas seleccionadas no se encuentran aptas para transferencia');
                                }
                                else if (data.data.listaFacturas[0].nCantFacturasRecepcionada != data.data.listaFacturas[0].nCantFacturasEvaluada) {
                                    messageError('Una o mas de una factura seleccionada no se encuentra en estado desembolsado.');
                                }
                            }
                        }
                        else {
                            messageError(data.mensaje);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        messageError(errorThrown);
                    }
                });
            }, 2000);
        }
    }



    var handleFormTransferir = function () {
        var form = document.getElementById('kt_modal_transfer_form');
        if (!form) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button');
        var validator;
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'Inversionista': {
                        validators: {
                            notEmpty: {
                                message: 'Inversionista es obligatorio'
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
        Transferencias.getRevalidateFormElement(form, 'Inversionista', validator);
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
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
                            data: {
                                IdFacturas: $('input:hidden[name="IdFacturas[]"]').val().split(','),
                                Inversionista: $('#Inversionista').val()
                            },
                            success: function (data) {
                                if (typeof data.processId != 'undefined') {
                                    if (data.processId !== 0) {
                                        Swal.fire({
                                            text: data.message,
                                            icon: 'success',
                                            buttonsStyling: false,
                                            confirmButtonText: 'Listo',
                                            customClass: {
                                                confirmButton: 'btn btn-primary'
                                            }
                                        }).then(function (result) {
                                            $(window).attr('location', globalPath + 'Transferencias');
                                        });
                                    } else {
                                        saveButton.removeAttribute('data-kt-indicator');
                                        saveButton.disabled = false;
                                        messageError('El servicio no esta disponible, intentar nuevamente.');
                                    }
                                }
                                else {
                                    saveButton.removeAttribute('data-kt-indicator');
                                    saveButton.disabled = false;
                                    messageError(data);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttribute('data-kt-indicator');
                                saveButton.disabled = false;
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





    var handleFormEnviarCavai = function () {
        var form = document.getElementById('kt_modal_accion_form');
        if (!form) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button_Accion');
        var validator;
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'InversionistaAccion': {
                        validators: {
                            notEmpty: {
                                message: 'Debe elegir que operación desea realizar'
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
        Transferencias.getRevalidateFormElement(form, 'InversionistaAccion', validator);
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
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
                            data: {
                                IdFacturasAccion: $('input:hidden[name="IdFacturasAccion[]"]').val().split(','),
                                InversionistaAccionRemover: $('#InversionistaAccionRemover').val(),
                                InversionistaAccion: $('#InversionistaAccion').val()

                            },
                            success: function (data) {
                                if (data.succeeded) {
                                    if (typeof data.data.valores.body.processId != 'undefined') {
                                        if (data.data.valores.body.processId !== 0) {
                                            Swal.fire({
                                                text: data.message,
                                                icon: 'success',
                                                buttonsStyling: false,
                                                confirmButtonText: 'Listo',
                                                customClass: {
                                                    confirmButton: 'btn btn-primary'
                                                }
                                            }).then(function (result) {
                                                $(window).attr('location', globalPath + 'Transferencias');
                                            });
                                        } else {
                                            saveButton.removeAttribute('data-kt-indicator');
                                            saveButton.disabled = false;
                                            messageError('El servicio no esta disponible, intentar nuevamente.');
                                        }
                                    }
                                    else {
                                        saveButton.removeAttribute('data-kt-indicator');
                                        saveButton.disabled = false;
                                        messageError(data.message);
                                    }
                                }
                                else {
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
                } else {
                    messageError('Lo sentimos, parece que se han detectado algunos errores. Vuelve a intentarlo.');
                }
            });

        });
    }

    var handleFormTraspasar = function () {
        var form = document.getElementById('kt_modal_traspasar_form');
        if (!form) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button_Traspaso');
        var validator;
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'InversionistaTraspaso': {
                        validators: {
                            notEmpty: {
                                message: 'Inversionista es obligatorio'
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
        Transferencias.getRevalidateFormElement(form, 'InversionistaTraspaso', validator);
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
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
                            data: {
                                IdFacturas: $('input:hidden[name="IdFacturasTraspaso[]"]').val().split(','),
                                Inversionista: $('#InversionistaTraspaso').val()
                            },
                            success: function (data) {
                                if (data != null) {
                                    if (typeof data.processId != 'undefined') {
                                        if (data.processId !== 0) {
                                            Swal.fire({
                                                text: data.message,
                                                icon: 'success',
                                                buttonsStyling: false,
                                                confirmButtonText: 'Listo',
                                                customClass: {
                                                    confirmButton: 'btn btn-primary'
                                                }
                                            }).then(function (result) {
                                                $(window).attr('location', globalPath + 'Transferencias');
                                            });
                                        } else {
                                            saveButton.removeAttribute('data-kt-indicator');
                                            saveButton.disabled = false;
                                            messageError('El servicio no esta disponible, intentar nuevamente.');
                                        }
                                    }
                                    else {
                                        saveButton.removeAttribute('data-kt-indicator');
                                        saveButton.disabled = false;
                                        messageError(data);
                                    }
                                }
                                else {
                                    saveButton.removeAttribute('data-kt-indicator');
                                    saveButton.disabled = false;
                                    messageError('El servicio no esta disponible, intentar nuevamente.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttribute('data-kt-indicator');
                                saveButton.disabled = false;
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

    var handleFormRemover = function () {
        var form = document.getElementById('kt_modal_remover_form');
        if (!form) {
            return;
        }
        var saveButton = document.getElementById('kt_save_button_Remover');
        var validator;
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'InversionistaRemover': {
                        validators: {
                            notEmpty: {
                                message: 'Inversionista es obligatorio'
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
        Transferencias.getRevalidateFormElement(form, 'InversionistaRemover', validator);
        saveButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
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
                            data: {
                                IdFacturas: $('input:hidden[name="IdFacturasRemover[]"]').val().split(','),
                                Inversionista: $('#InversionistaRemover').val()
                            },
                            success: function (data) {
                                if (data != null) {
                                    if (typeof data.processId != 'undefined') {
                                        if (data.processId !== 0) {
                                            Swal.fire({
                                                text: data.message,
                                                icon: 'success',
                                                buttonsStyling: false,
                                                confirmButtonText: 'Listo',
                                                customClass: {
                                                    confirmButton: 'btn btn-primary'
                                                }
                                            }).then(function (result) {
                                                $(window).attr('location', globalPath + 'Transferencias');
                                            });
                                        } else {
                                            saveButton.removeAttribute('data-kt-indicator');
                                            saveButton.disabled = false;
                                            messageError('El servicio no esta disponible, intentar nuevamente.');
                                        }
                                    }
                                    else {
                                        saveButton.removeAttribute('data-kt-indicator');
                                        saveButton.disabled = false;
                                        messageError(data);
                                    }
                                }
                                else {
                                    saveButton.removeAttribute('data-kt-indicator');
                                    saveButton.disabled = false;
                                    messageError('El servicio no esta disponible, intentar nuevamente.');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                saveButton.removeAttribute('data-kt-indicator');
                                saveButton.disabled = false;
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
    var handleRevalidateFormElement = function (form, element, validator) {
        $(form.querySelector('[name="' + element + '"]')).on('change', function () {
            validator.revalidateField(element);
        });
    }
    return {
        init: function () {
            handleFilterTable();
            initDatatable();
            handleFormTransferir();
            handleFormTraspasar();
            handleFormRemover();
            handleFormEnviarCavai();
        },
        getRevalidateFormElement: function (form, elem, val) {
            handleRevalidateFormElement(form, elem, val);
        }
    }
}();
KTUtil.onDOMContentLoaded(function () {
    Transferencias.init();
});