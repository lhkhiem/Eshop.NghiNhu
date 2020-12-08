var unit = {
    init: function () {
        unit.loadData();
        unit.registerEvent();
    },
    registerEvent: function () {
    },

    loadData: function () {
        $.ajax({
            url: '/Unit/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var stt = 1;
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            STT: stt,
                            ID:item.ID,
                            Name: item.Name
                        });
                        stt++;
                    });
                    $('#unitTableBody').html(html);
                    //$('#lbTotal').text(response.totalCurent + " / " + response.total + ' tổng số bản ghi');
                }
            }
        })
    }
}
unit.init();