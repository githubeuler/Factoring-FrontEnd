'use strict';
//comentario de prueba
var globalPath = $('base').attr('href');

var AccountSignin = function () {
    var validator;





    var handleFormChangePassword = function (e) {

        var form = document.querySelector('#kt_change_password_form');
        var submitButton = document.querySelector('#kt_change_password_submit');

        //const passwordMeter = document.getElementById('passwordMeterNew');
        //const randomNumber = function (min, max) {
        //    return Math.floor(Math.random() * (max - min + 1) + min);
        //};
        
        validator = FormValidation.formValidation(
            form,
            {
                fields: {
                    'CurrentPassword': {
                        validators: {
                            notEmpty: {
                                message: 'Contraseña actual es obligatorio'
                            }
                        }
                    },
                    'NewPassword': {
                        validators: {
                            notEmpty: {
                                message: 'Nueva contraseña es obligatorio'
                            },
                            stringLength: {
                                min: 8,
                                message: 'Contraseña debe tener minimo 8 caracteres'
                            }
                        }
                    },
                    'ConfirmPassword': {
                        validators: {
                            notEmpty: {
                                message: 'Confirmar nueva contraseña es obligatorio'
                            },
                            stringLength: {
                                min: 8,
                                message: 'Contraseña debe tener minimo 8 caracteres'
                            },
                            identical: {
                                compare: function () {
                                    return document.getElementById('NewPassword').value;
                                },
                                message: 'Las contraseñas no coinciden'
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
                    }),
                    //passwordStrength: new FormValidation.plugins.PasswordStrength({
                    //    field: 'pwd',
                    //    message: 'The password is weak',
                    //    minimalScore: 3,
                    //    onValidated: function (valid, message, score) {
                    //        // Set the styles for password meter element
                    //        // based on the score
                    //        switch (score) {
                    //            case 0:
                    //                passwordMeter.style.width = randomNumber(1, 20) + '%';
                    //                passwordMeter.style.backgroundColor = '#ff4136';
                    //            case 1:
                    //                passwordMeter.style.width = randomNumber(20, 40) + '%';
                    //                passwordMeter.style.backgroundColor = '#ff4136';
                    //                break;
                    //            case 2:
                    //                passwordMeter.style.width = randomNumber(40, 60) + '%';
                    //                passwordMeter.style.backgroundColor = '#ff4136';
                    //                break;
                    //            case 3:
                    //                passwordMeter.style.width = randomNumber(60, 80) + '%';
                    //                passwordMeter.style.backgroundColor = '#ffb700';
                    //                break;
                    //            case 4:
                    //                passwordMeter.style.width = '100%';
                    //                passwordMeter.style.backgroundColor = '#19a974';
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //    },
                    //})
                }
            }
        )
        submitButton.addEventListener('click', function (e) {
            e.preventDefault();
            validator.validate().then(function (status) {
                if (status == 'Valid') {
                    submitButton.setAttribute('data-kt-indicator', 'on');
                    submitButton.disabled = true;
                    setTimeout(function () {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            url: $('#kt_change_password_form').attr('action'),
                            xhrFields: {
                                withCredentials: true
                            },
                            data: $('#kt_change_password_form').serializeObject(),
                            success: function (data) {
                               
                                if (data.succeeded) {
                                   
                                //    //$(window).attr('location', globalPath);
                                //    /*
                                //     * Permite que el navegador no pueda dar click en el boton atras e ir nuevamente al login
                                //     */
                                //    if (data.data.mustChangePassword == 1) {
                                //        window.location.replace(globalPath + 'Account/ChangePassword')
                                //    } else {
                                //        window.location.replace(globalPath)
                                //    }
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
                                            window.location.replace(globalPath)
                                            //$('#kt_modal_registro_datos_fondeo').modal('hide');
                                        }
                                    });

                                } else {
                                    submitButton.removeAttribute('data-kt-indicator');
                                    submitButton.disabled = false;
                                    messageError(data.message);
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                submitButton.removeAttribute('data-kt-indicator');
                                submitButton.disabled = false;
                                messageError(errorThrown);
                            }
                        });

                    }, 2000);
                } else {
                    //messageError('Favor de ingresar los campos obligatorios y volver a intentarlo.');
                }
            });
        });
    }


    return {
        init: function () {
            handleFormChangePassword();
       
        }
    };
}();
KTUtil.onDOMContentLoaded(function () {
    AccountSignin.init();
});