var productCategory = {
    init: function () {
        productCategory.registerEvent();
    },
    registerEvent: function () {
        $('#btnImage').on('click', function (e) {
            e.preventDefault();
            var fider = new CKFinder();
            fider.selectActionFunction = function (url) {
                $('#txtImage').val(url);
                $('#imgCategory').attr('src', url);
            };
            fider.popup();
        });
        $('#txtName').off('keyup').on('keyup', function (e) {
            var str = $('#txtName').val();
            productCategory.convertString(str);
        });
        $('#txtMetaTitle').off('click').on('click', function (e) {
            var str = $('#txtName').val();
            productCategory.convertString(str);
        });
    },
    convertString: function (str) {
        $.ajax({
            url: '/ProductCategory/ConvertString',
            data: {
                str: str
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                $('#txtMetaTitle').val(response.str);
            }
        });
    },
}
productCategory.init();