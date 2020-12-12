var productCategory = {
    init: function () {
        productCategory.loadData();
        productCategory.registerEvent();
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
                                productCategory.delete(id);
                            }
                        });
                    }
                });
            }
            else {
                bootbox.alert("Vui lòng chọn đối tượng cần xóa!");
            }
        });
        $('#tableBody').on('click', '.btn-active', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/ProductCategory/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.removeClass('badge-danger');
                        btn.addClass('badge-success');
                        //productCategory.loadData();
                    }
                    else {
                        btn.removeClass('badge-success');
                        btn.addClass('badge-danger');
                        //productCategory.loadData();
                    }
                }
            });
        });
        $('#tableBody').on('change', '.sltOrder', function (e) {
            e.preventDefault();
            var slt = $(this);
            var id = slt.data('id');
            var order = slt.val();
            $.ajax({
                url: "/ProductCategory/ChangeOrder",
                data: {
                    id: id,
                    order: order,
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        productCategory.loadData();
                    }
                    else {
                        productCategory.loadData();
                    }
                }
            });
        });
    },

    dateFormat: function (d) {
        return ((d.getMonth() + 1) + "").padStart(2, "0")
            + "/" + (d.getDate() + "").padStart(2, "0")
            + "/" + d.getFullYear();
    },
    loadData: function () {
        $.ajax({
            url: '/ProductCategory/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    var template2 = $('#data-template2').html();
                    $.each(data, function (i, item) {
                        if (item.ParentID == 0) {
                            html += Mustache.render(template, {
                                ID: item.ID,
                                Name: item.Name,
                                Order: item.DisplayOrder,
                                MetaTitle: item.MetaTitle,
                                CreateDate: productCategory.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                                CreateBy: item.CreateBy,
                                Status: item.Status == true ? "<a href=\"#\" data-id=" + item.ID + " class=\"badge badge-success btn-active\">Active</a>" : "<a href=\"#\" data-id=" + item.ID + " class=\"badge badge-danger btn-active\">Lock</a>"
                            });
                        }
                        else {
                            html += Mustache.render(template2, {
                                ID: item.ID,
                                Name: ('-- ' + item.Name),
                                Order: item.DisplayOrder,
                                MetaTitle: item.MetaTitle,
                                CreateDate: productCategory.dateFormat(new Date(parseInt((item.CreateDate).match(/\d+/)[0]))),
                                CreateBy: item.CreateBy,
                                Status: item.Status == true ? "<a href=\"#\" data-id=" + item.ID + " class=\"badge badge-success btn-active\">Active</a>" : "<a href=\"#\" data-id=" + item.ID + " class=\"badge badge-danger btn-active\">Lock</a>"
                            });
                        }
                    });
                    $('#tableBody').html(html);
                }
            }
        })
    },
    delete: function (id) {
        $.ajax({
            url: '/ProductCategory/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == 0) bootbox.alert("Lỗi không thể xóa!");
                else if (response.status == 2) bootbox.alert("Đối tượng đang được sử dụng!");
                else if (response.status == 3) bootbox.alert("Xóa hết danh mục con trước khi xóa danh mục này!");
                else location.reload();
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
}
productCategory.init();