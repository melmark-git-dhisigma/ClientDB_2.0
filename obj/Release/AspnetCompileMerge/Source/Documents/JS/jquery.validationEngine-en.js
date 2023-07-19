(function ($) {
    $.fn.validationEngineLanguage = function () {
    };
    $.validationEngineLanguage = {
        newLang: function () {
            $.validationEngineLanguage.allRules = {
                "required": { // Add your regex rules here, you can take telephone as an example
                    "regex": "none",
                    "alertText": "* This field is required",//"* This field is required",
                    "alertTextCheckboxMultiple": "* Please select an option",
                    "alertTextCheckboxe": "* يجب تعبئة هذا الفراغ",
                    "alertTextDateRange": "* Both date range fields are required"
                },
                "requiredInFunction": {
                    "func": function (field, rules, i, options) {
                        var password = field.val();
                       // var passArray = password.split("");
                        var capsCount = 0;
                        var smallCount = 0;
                        var digitCount = 0;
                        var symbolCount = 0;

                        //alert(password);
                        for (var i = 0; i < password.length; i++) {

                            var charcode = password.charCodeAt(i);
                            if (charcode >= 65 && charcode <= 90) {
                                capsCount++;
                            }
                            if (charcode >= 97 && charcode <= 122) {
                                smallCount++;
                            }
                            if (charcode >= 48 && charcode <= 57) {
                                digitCount++;
                            }
                            if (charcode >= 33 && charcode <= 47) {
                                symbolCount++;

                            }
                            if (charcode >= 58 && charcode <= 64) {
                                symbolCount++;
                            }
                            if (charcode >= 91 && charcode <= 96) {
                                symbolCount++;
                            }
                            if (charcode >= 123 && charcode <= 126) {
                                symbolCount++;
                            }

                        }
                        if (capsCount >= 1 && smallCount >= 3 && digitCount >= 4 && symbolCount >= 1) {

                            return true;

                        }
                        else {

                            return false;

                        }

                    },
                    "alertText": "*كلمة السر غير صالحة: كلمة السر لا تفي بالمعايير المطلوبة"
                },//^(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}$

                "phoneNumber": {
                    "func": function (field, rules, i, options) {
                        var textValue = field.val();
                        //^(\([0-9]{3}\)|[0-9]{3}-)[0-9]{3}-[0-9]{4}$
                        var testValue = /(\([0-9]{3}\) |[0-9]{3}-)[0-9]{3}-[0-9]{4}/;
                        for (var i = 0; i < textValue.length; i++) {
                            if (!testValue.test(textValue[i])) {
                                return false;
                            }
                        }

                        return true;

                    },

                    "alertText": "Invalid Phone Number "
                },
                "checkUsername": {
                    "func": function (field, rules, i, options) {
                        var textValue = $(field).css('border-left-color');

                        //alert(textValue);

                        if (textValue == "rgb(255, 0, 0)") {
                            return false;
                        }
                        //else if (textValue == "rgb(0, 128, 0)") {
                        //    return true;
                        //}
                        return true;
                    },

                    "alertText": "Username already exist"
                },
                //[\(\d]{3}\)-[\d]{3}-[\d]{4}$ ^\([0-9]{3}\)[0-9]{3}\-[0-9]{4}$ 
                //^(\+?1-?)?(\([0-9]\d{3}\)|[0-9]\d{3})-?[0-9]\d{4}-?\d{4}
       
                "usPhoneNumber": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^\([0-9]{3}\)[0-9]{3}\-[0-9]{4}$/,
                    "alertText": "Invalid Phone Number"
                },
                "usFaxNumber": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^\([0-9]{3}\)[0-9]{3}\-[0-9]{4}$/,
                    "alertText": "Invalid Fax Number"
                },
                "arabicOnly": {
                    "func": function (field, rules, i, options) {
                        var textValue = field.val();
                        var arabicValue = /[\u0600-\u06FF\ ]/;
                        for (var i = 0; i < textValue.length; i++) {
                            if(!arabicValue.test(textValue[i]))
                            {
                                return false;
                            }
                        }

                        return true;

                    },
                   
                    "alertText": "* يسمح  للنص العربي  فقط "
                },

                "arabicOnlywithspaces": {
                    "func": function (field, rules, i, options) {
                        var textValue = field.val().trim();
                        var splittext = textValue.split(" ");
                       // var count = (textValue.split(" ").length - 1);
                        var count = 0;
                        for (var i = 0; i < splittext.length; i++)
                        {
                            if (splittext[i] != "")
                                count++;
                        }
                        
                        if (count < 3)
                            return false;
                       
                        var arabicValue = /[\u0600-\u06FF\ ]/;
                        for (var i = 0; i < textValue.length; i++) {
                            if (!arabicValue.test(textValue[i])) {
                                return false;
                            }
                        }
                       return true;

                    },

                    "alertText": "* أحمد سالم حمدان "
                },

                "englishOnlywithspaces": {
                    "func": function (field, rules, i, options) {
                        var textValue = field.val().trim();
                      
                        var englishValue = /[a-zA-Z\ ]/;
                        for (var i = 0; i < textValue.length; i++) {
                            
                            if (!englishValue.test(textValue[i])) {
                                return false;
                            }
                        }
                        return true;

                    },

                    "alertText": "* Invalid Charecters"
                },

                "other": {
                    "func": function (field, rules, i, options) {
                        var textValue = field.val();
                        
                        if (textValue=='-1') {
                                return false;
                            }
                        return true;
                    },

                    "alertText": "*يجب تعبئة هذا الفراغ"
                },
  
                "dateRange": {
                    "regex": "none",
                    "alertText": "* Invalid ",
                    "alertText2": "Date Range"
                },
                "dateTimeRange": {
                    "regex": "none",
                    "alertText": "* Invalid ",
                    "alertText2": "Date Time Range"
                },
                "minSize": {
                    "regex": "none",
                    "alertText": "* Minimum ",
                    "alertText2": " characters required"
                },
                "maxSize": {
                    "regex": "none",
                    "alertText": "* Maximum ",
                    "alertText2": " characters allowed"
                },
                "groupRequired": {
                    "regex": "none",
                    "alertText": "* You must fill one of the following fields"
                },
                "min": {
                    "regex": "none",
                    "alertText": "* Minimum value is "
                },
                "max": {
                    "regex": "none",
                    "alertText": "* Maximum value is "
                },
                "past": {
                    "regex": "none",
                    "alertText": "* Date prior to "
                },
                "future": {
                    "regex": "none",
                    "alertText": "* Date past "
                },
                "maxCheckbox": {
                    "regex": "none",
                    "alertText": "* Maximum ",
                    "alertText2": " options allowed"
                },
                "minCheckbox": {
                    "regex": "none",
                    "alertText": "* Please select ",
                    "alertText2": " options"
                },
                "equals": {
                    "regex": "none",
                    "alertText": "*البيانات لا تطابق"
                },
                "creditCard": {
                    "regex": "none",
                    "alertText": "* Invalid credit card number"
                },
                "phone": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([\+][0-9]{1,3}[\ \.\-])?([\(]{1}[0-9]{2,6}[\)])?([0-9\ \.\-\/]{3,20})((x|ext|extension)[\ ]?[0-9]{1,4})?$/,
                    "alertText": "* رقم غير صالح"
                },
                "email": {
                    "regex": /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
                    "alertText": "*  Invalid Email "
                },
                "integer": {
                    "regex": /^[\-\+]?\d+$/,
                    "alertText": "*رقم/عدد أرقام غير صالحة"
                },
                "number": {
                    // Number, including positive, negative, and floating decimal. credit: orefalo
                    "regex": /^[\-\+]?((([0-9]{1,3})([,][0-9]{3})*)|([0-9]+))?([\.]([0-9]+))?$/,
                    "alertText": "* رقم عشري عائم غير صحيح"
                },
                "float": {
                    // Number, including positive, negative, and floating decimal. credit: orefalo
                    "regex": /^((([0-9]{1,3})([,][0-9]{3})*)|([0-9]+))?([\.]([0-9]+))?$/,
                    "alertText": "* رقم عشري عائم غير صحيح"
                },
                "mobileNo": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([0-9\u0660-\u0669]{7})$/,
                    "alertText": "* (+971-xx-xxxxxxx) رقم الهاتف المتحرك غير صالح"
                },
                "phoneNo": {
                    // credit: jquery.h5validate.js / orefalo\u0660-\u0669
                    "regex": /^([0-9\u0660-\u0669]{7})$/,
                    "alertText": "*  (+971-xx-xxxxxxx)  رقم غير صالح"
                },
                "emirateIdFirst": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([0-9\u0660-\u0669]{3})$/,
                    "alertText": "* رقم الهوية غير صالح (xxx-xxxx-xxxxxxx-x)"
                },
              
                "emirateIdSecond": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([0-9\u0660-\u0669]{4})$/,
                    "alertText": "* رقم الهوية غير صالح (xxx-xxxx-xxxxxxx-x)"
                },
                "emirateIdThird": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([0-9\u0660-\u0669]{7})$/,
                    "alertText": "* رقم الهوية غير صالح (xxx-xxxx-xxxxxxx-x)"
                },
                "emirateIdFourth": {
                    // credit: jquery.h5validate.js / orefalo
                    "regex": /^([0-9\u0660-\u0669]{1})$/,
                    "alertText": "* رقم الهوية غير صالح (xxx-xxxx-xxxxxxx-x)"
                },
                "townNo": {
                    "regex": /^([0-9\u0660-\u0669]{2})$/,
                    "alertText": "*رقم البلدة غير صالح(3xx)"
                },
                "faxNo": {
                    "regex": /^([0-9\u0660-\u0669]{7})$/,
                    "alertText": "*  (+971-xx-xxxxxxx)  رقم غير صالح"
                },
                "familyNo": {
                    "regex": /^([0-9\u0660-\u0669]{3,5})$/,
                    "alertText": "*يجب أن يكون الحد الأقصى للأرقام المدخلة 5 والحد الأدنى هو 3 أرقام"
                },
                "noOfWives": {
                    "regex": /^([0-4]{1})$/,
                    "alertText": "*مجموع عدد الزوجات لا يتجاوز 4"
                },
                "date": {
                    //	Check if date is valid by leap year
                    "func": function (field) {
                        var pattern = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[\/\-\.](0?[1-9]|1[012])[\/\-\.](\d{4})$/);
                        var match = pattern.exec(field.val());
                        if (match == null)
                            return false;

                        var year = match[3];
                        var month = match[2] * 1;
                        var day = match[1] * 1;
                        var date = new Date(year, month - 1, day); // because months starts from 0.

                        return (date.getFullYear() == year && date.getMonth() == (month - 1) && date.getDate() == day);
                    },
                    "alertText": "*تاريخ غير صالح، يجب ادخاله على النمط التالي: يوم/شهر/سنة"
                },
                "passwordCheck": {
                    //	Check for the password format
                    "func": function (field) {
                        var p = field.val(),
     errors = [];
                        if (p.search(/(?:\D*\d){4}/g) < 0) {
                            errors.push("Your password must be at least 8 characters");
                            return false;
                        }

                        return true;
                    },
                    "alertText": "* Invalid password format"
                },
                "ipv4": {
                    "regex": /^((([01]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))[.]){3}(([0-1]?[0-9]{1,2})|(2[0-4][0-9])|(25[0-5]))$/,
                    "alertText": "* Invalid IP address"
                },
                "url": {
                    "regex": /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i,
                    "alertText": "* Invalid URL"
                },
                "onlyNumberSp": {
                    "regex": /^[0-9\ ]+$/,
                    "alertText": "*  أرقام فقط"
                },
                "onlyLetterSp": {
                    "regex": /^[a-zA-Z\ ]+$/,
                    "alertText": "* أحرف فقط"
            },
           
                "onlyLetter": {
                    "regex": /^[a-zA-Z]+$/,
                    "alertText": "* أحرف فقط"
                },
                "username": {
                    "regex": /^[a-zA-Z]+$/,
                    "alertText": "* أحرف فقط"
                },
                "poBox": {
                    "regex": /^(\d)+(\-)([\u0600-\u06FF\ ]+$)$/,
                    "alertText": "* صندوق بريد غير صالح"
                },
                "onlyLetterNumber": {
                    "regex": /^[0-9a-zA-Z]+$/,
                    "alertText": "* No special characters allowed"
                },
                // --- CUSTOM RULES -- Those are specific to the demos, they can be removed or changed to your likings
                "ajaxUserCall": {
                    "url": "ajaxValidateFieldUser",
                    // you may want to pass extra data on the ajax call
                    "extraData": "name=eric",
                    "alertText": "* This user is already taken",
                    "alertTextLoad": "* Validating, please wait"
                },
                "ajaxUserCallPhp": {
                    "url": "phpajax/ajaxValidateFieldUser.php",
                    // you may want to pass extra data on the ajax call
                    "extraData": "name=eric",
                    // if you provide an "alertTextOk", it will show as a green prompt when the field validates
                    "alertTextOk": "* This username is available",
                    "alertText": "* This user is already taken",
                    "alertTextLoad": "* Validating, please wait"
                },
                "ajaxNameCall": {
                    // remote json service location
                    "url": "ajaxValidateFieldName",
                    // error
                    "alertText": "* This name is already taken",
                    // if you provide an "alertTextOk", it will show as a green prompt when the field validates
                    "alertTextOk": "* This name is available",
                    // speaks by itself
                    "alertTextLoad": "* Validating, please wait"
                },
                "ajaxNameCallPhp": {
                    // remote json service location
                    "url": "phpajax/ajaxValidateFieldName.php",
                    // error
                    "alertText": "* This name is already taken",
                    // speaks by itself
                    "alertTextLoad": "* Validating, please wait"
                },
                "validate2fields": {
                    "alertText": "* Please input HELLO"
                },
                //tls warning:homegrown not fielded 
                "dateFormat": {
                    "regex": /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$|^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(?:(?:0?[1-9]|1[0-2])(\/|-)(?:0?[1-9]|1\d|2[0-8]))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^(0?2(\/|-)29)(\/|-)(?:(?:0[48]00|[13579][26]00|[2468][048]00)|(?:\d\d)?(?:0[48]|[2468][048]|[13579][26]))$/,
                    "alertText": "* Invalid Date"
                },
                //tls warning:homegrown not fielded 
                "dateTimeFormat": {
                    "regex": /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}\s+(am|pm|AM|PM){1}$|^(?:(?:(?:0?[13578]|1[02])(\/|-)31)|(?:(?:0?[1,3-9]|1[0-2])(\/|-)(?:29|30)))(\/|-)(?:[1-9]\d\d\d|\d[1-9]\d\d|\d\d[1-9]\d|\d\d\d[1-9])$|^((1[012]|0?[1-9]){1}\/(0?[1-9]|[12][0-9]|3[01]){1}\/\d{2,4}\s+(1[012]|0?[1-9]){1}:(0?[1-5]|[0-6][0-9]){1}:(0?[0-6]|[0-6][0-9]){1}\s+(am|pm|AM|PM){1})$/,
                    "alertText": "* Invalid Date or Date Format",
                    "alertText2": "Expected Format: ",
                    "alertText3": "mm/dd/yyyy hh:mm:ss AM|PM or ",
                    "alertText4": "yyyy-mm-dd hh:mm:ss AM|PM"
                }
            };

        }
    };

    $.validationEngineLanguage.newLang();

})(jQuery);
