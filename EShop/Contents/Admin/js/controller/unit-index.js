var unit = {
    init: function () {
        unit.loadData();
        unit.registerEvent();
    },
    registerEvent: function () {
        $('#customCheckboxAll').off('click').on('click', function () {
            if ($(this).prop("checked")) {
                $('.check-item').each(function () {
                    $(this).prop('checked', true);
                });
            }
            else {
                $('.check-item').each(function () {
                    $(this).prop('checked', false);
                });
            }

        });
        $('.btn-delete').off('click').on('click', function () {
            var hasCheck = false;
            $('.check-item').each(function () {
                if ($(this).prop("checked")) hasCheck = true;

            });
            if (hasCheck) {
                bootbox.confirm("Bạn có muốn xóa không?", function (result) {
                    if (result) {
                        $('.check-item').each(function () {
                            if ($(this).prop("checked")) {
                                var id = $(this).data('id');
                                unit.delete(id);
                                location.reload();
                                //$('#productCategoryTable').ajax.reload(null, false);
                            }
                        });
                    }
                });
                
            }
            else {
                bootbox.alert("Vui lòng chọn đối tượng cần xóa!");
            }
        });
    },

    loadData: function () {
        $.ajax({
            url: '/Unit/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.ID,
                            Name: item.Name
                        });
                    });
                    $('#tableBody').html(html);
                }
            }
        })
    },
    delete: function (id) {
        $.ajax({
            url: '/Unit/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == 0) bootbox.alert("Lỗi không thể xóa!");
                else if (response.status == 2) bootbox.alert("Đối tượng đang được sử dụng!");
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
}
unit.init();