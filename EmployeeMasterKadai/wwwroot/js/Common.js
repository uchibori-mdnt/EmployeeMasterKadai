
function scheduleTime() {
    var dataList = document.getElementById("data-list");
    // 9:00から18:00までの時間を生成してdatalistに挿入
    for (var hour = 9; hour <= 18; hour++) {
        for (var minute = 0; minute < 60; minute += 15) {
            var option = document.createElement("option");
            if (hour != 18) {
                var time = pad(hour) + ":" + pad(minute);
                option.value = time; 
                dataList.appendChild(option); 
            }
            else {
                var time = "18:00"; 
                option.value = time;
                dataList.appendChild(option);
                return;
            }
        }
    }
    function pad(number) {
        return (number < 10 ? "0" : "") + number;
    }
}

//モーダル画面の確認ボタン
var submitButtonHtml = '<div class="d-flex justify-content-end pb-4">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">キャンセル</a>' +
    '</div>' +
    '<div class="p-2">' +
    '<input type="submit" value="登録" class="btn btn-primary" />' +
    '</div>' +
    '</div>';

$('#modalFooter').html(submitButtonHtml);

// 登録ボタンorキャンセルボタンが押された時の処理
$('#modalFooter').on('click', 'input[type="submit"]', function () {
    var clickedButtonValue = $(this).val();
    var startDateInput = document.getElementById("startDate");

    if (clickedButtonValue === "登録") {
        if (startDateInput != null) {
            combineDateTime()
        }
        $('form').submit();

    } else if (clickedButtonValue === "キャンセル") {
        $('#externalModal').modal('hide');
    }
});

//スケジュール日付時間を合体
function combineDateTime() {
    var startDateInput = document.getElementById("startDate");
    var startTimeInput = document.getElementById("startTime");
    var endDateInput = document.getElementById("endDate");
    var endTimeInput = document.getElementById("endTime");

    if (startDateInput != null && endDateInput != null && startTimeInput != null && endTimeInput != null) {
        var startDate = startDateInput.value;
        var startTime = startTimeInput.value;

        var endDate = endDateInput.value;
        var endTime = endTimeInput.value;

        var combinedStart = startDate + " " + startTime;
        var combinedEnd = endDate + " " + endTime;

        $('#StartDay').val(combinedStart);
        $('#EndDay').val(combinedEnd);
    }
}


//スケジュール日付時間を解体、入力フィールドへ表示
function setEditDateTime() {
    var startDayValue = $('#StartDay').val();
    var endDayValue = $('#EndDay').val();
    console.log(startDayValue)

    if (startDayValue) {
        var castStartValue = new Date(startDayValue);
        var castendValue = new Date(endDayValue);

        var startDateValue = castStartValue.getFullYear() + '-' + ('00' + (castStartValue.getMonth() + 1)).slice(-2) + '-' + ('00' + castStartValue.getDate()).slice(-2);
        var startTimeValue = ('00' + castStartValue.getHours()).slice(-2) + ':' + ('00' + castStartValue.getMinutes()).slice(-2);
        var endDateValue = castendValue.getFullYear() + '-' + ('00' + (castendValue.getMonth() + 1)).slice(-2) + '-' + ('00' + castendValue.getDate()).slice(-2);
        var endTimeValue = ('00' + castendValue.getHours()).slice(-2) + ':' + ('00' + castendValue.getMinutes()).slice(-2);

        if (startTimeValue === "00:00") {
            $('#startDate').val(startDateValue);
            $('#endDate').val(endDateValue);
        }
        else {
            $('#startDate').val(startDateValue);
            $('#startTime').val(startTimeValue);

            $('#endDate').val(endDateValue);
            $('#endTime').val(endTimeValue);
        }
    }
}

//確認メッセージのはいいいえ
var checkButtonHtml = '<div class="d-flex justify-content-end pb-4">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">いいえ</a>' +
    '</div>' +
    '<div class="p-2">' +
    '<input type="submit" value="はい" class="btn btn-primary" />' +
    '</div>' +
    '</div>';

//もう一度入力のボタン
var yesButtonHtml = '<div class="p-2">' +
    '<input type="submit" value="もう一度入力" class="btn btn-primary" />' +
    '</div>';

//エラーメッセージの閉じるボタン
var errorCloseButton = '<div class="d-flex justify-content-end pb-4">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">閉じる</a>' +
    '</div>' +
    '</div>';

function showConfirmationModal(action, message, callback) {
    $('#messageModal').modal('show');
    $('#messageLavel').text('確認');
    $('#modalBody').text(message);
    $('#closeModalBtn').html(checkButtonHtml);

    $('#closeModalBtn').on('click', 'input[type="submit"]', function () {
        var checkedButtonValue = $(this).val();

        if (checkedButtonValue === "はい") {
            callback();
        } else if (checkedButtonValue === "いいえ") {
        }
        $('#externalModal').modal('hide');
    });
}


//エラーモーダル
function showErrorModal() {
    $('#messageModal').modal('show');
    $('#messageLavel').text('エラー');
    $('#modalBody').text('登録できませんでした。画面を確認してください。');
    $('#closeModalBtn').html(errorCloseButton);

    $('#closeModalBtn').on('click', 'input[type="submit"]', function () {
        var checkedButtonValue = $(this).val();

        if (checkedButtonValue === "閉じる") {
            $('#externalModal').modal('hide');
            connectTOForm();
        }
    });
}