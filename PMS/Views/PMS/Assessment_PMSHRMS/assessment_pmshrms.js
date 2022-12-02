
    $(function () {


        //$('#checker').on('change', function () {
        //    showHidediv();
        //    alert('');
        //});
        $('#checker').on('change', function () {
            var a = $(this).val();
            //console.log(a);
            if (a == '1') {
                $('#form_type2').hide();
                $('#form_type1').show();
            } else if (a == '2') {

                $('#form_type1').show();
                $('#form_type2').hide();
            } else if (a == '3') {

                $('#form_type1').hide();
                $('#form_type2').show();
            } else {
                $('#form_type1, #form_type2').hide();
            }
        });
        $('.optionMinus').on('click', function () {
            alert('');
            $(this).closest('tr').remove();
        });
        $('.optionAdd').on('click', function () {
            var thisTr = $(this).closest('tr');
            $(this).closest('.tr_clone')
                    .clone()
                    .insertAfter(thisTr);


        })
    });
