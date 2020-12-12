"use strict";

$("[data-checkboxes]").each(function () {
    var me = $(this),
        group = me.data('checkboxes'),
        role = me.data('checkbox-role');

    me.change(function () {
        var all = $('[data-checkboxes="' + group + '"]:not([data-checkbox-role="dad"])'),
            checked = $('[data-checkboxes="' + group + '"]:not([data-checkbox-role="dad"]):checked'),
            dad = $('[data-checkboxes="' + group + '"][data-checkbox-role="dad"]'),
            total = all.length,
            checked_length = checked.length;

        if (role == 'dad') {
            if (me.is(':checked')) {
                all.prop('checked', true);
            } else {
                all.prop('checked', false);
            }
        } else {
            if (checked_length >= total) {
                dad.prop('checked', true);
            } else {
                dad.prop('checked', false);
            }
        }
    });
});

$("#table-1").dataTable({
    "columnDefs": [
        { "sortable": false, "targets": [2, 3] }
    ]
});
$("#table-2").dataTable({
    "columnDefs": [
        { "sortable": false, "targets": [0, 2, 3] }
    ],
    order: [[1, "asc"]] //column indexes is zero based
});
$('#save-stage').DataTable({
    "scrollX": true,
    stateSave: true
});
$('#tableExport').DataTable({
    dom: 'Bfrtip',
    buttons: [
        'copy', 'csv', 'excel', 'pdf', 'print'
    ],
});
//Custom datatable 
$('#table').DataTable({
    'dom': 'flrtip',
    "order": [],
    'columnDefs': [
        {
        'targets': [0], // column index (start from 0)
        'orderable': false, // set orderable false for selected columns
        }
    ],
    //Cấu hình cho ngôn ngữ tiếng việt với Bootstrap icon
    "language": {
        "sLengthMenu": "Hiển thị _MENU_ dòng mỗi trang",
        "oPaginate": {
            "sFirst": "",
            "sLast": "",
            "sNext": "",
            "sPrevious": ""
        },
        "sEmptyTable": "Không có dữ liệu",
        "sSearch": "Tìm kiếm:",
        "sZeroRecords": "Không có dữ liệu",
        "sInfo": "Hiển thị từ _START_ đến _END_ trong tổng số _TOTAL_ dòng được tìm thấy",
        "sInfoEmpty": "Không tìm thấy",
        "sInfoFiltered": " (trong tổng số _MAX_ dòng)",
        "processing": "Đang tải dữ liệu!"
    },
});