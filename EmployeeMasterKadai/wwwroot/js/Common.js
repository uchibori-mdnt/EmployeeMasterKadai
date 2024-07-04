function scheduleTime() {
    const dataList = $("#data-list");
    // 9:00から17:45までの時間を生成してdatalistに挿入
    for (let hour = 9; hour < 18; hour++) {
        for (let minute = 0; minute < 60; minute += 15) {
            const option = document.createElement("option");
            const time = pad(hour) + ":" + pad(minute);
            option.value = time;
            dataList.append(option);
        }
    }
    // 18:00を1回だけ追加
    const option = document.createElement("option");
    option.value = "18:00";
    option.text = "18:00";
    dataList.append(option);

    function pad(number) {
        return (number < 10 ? "0" : "") + number;
    }
    displayDatalist();
    displayEndDatalist();
}

// 初期化関数を呼び出す
$(document).ready(function () {
    scheduleTime();
});

function displayDatalist() {
    let temp = "";
    $("#startTime").mousedown(function () {
        temp = $("#startTime").val();
        //入力を削除する
        $("#startTime").val("");
    });
    //候補が表示された後に入力値を入れ直す
    $("#startTime").click(function () {
        if (temp !== "") {
            $("#startTime").val(temp);
        }
    });
}

function displayEndDatalist() {
    let temp = "";
    $("#endTime").mousedown(function () {
        temp = $("#endTime").val();
        $("#endTime").val("");
    });

    $("#endTime").click(function () {
        if (temp !== "") {
            $("#endTime").val(temp);
        }
    });
}

//モーダル画面の確認ボタン
const submitButtonHtml = '<div class="d-flex justify-content-end">' +
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
    const clickedButtonValue = $(this).val();
    const startDateInput = $("#startDate");

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
    const startDateInput = $("#startDate");
    const startTimeInput = $("#startTime");
    const endDateInput = $("#endDate");
    const endTimeInput = $("#endTime");

    if (startDateInput.length && startTimeInput.length && endDateInput.length && endTimeInput.length) {
        const startDate = startDateInput.val();
        const startTime = startTimeInput.val();
        const endDate = endDateInput.val();
        const endTime = endTimeInput.val();

        const combinedStart = startDate + " " + startTime;
        const combinedEnd = endDate + " " + endTime;

        $('#StartDay').val(combinedStart);
        $('#EndDay').val(combinedEnd);
    }
}

//スケジュール日付時間を解体、入力フィールドへ表示
function setEditDateTime() {
    const startDayValue = $('#StartDay').val();
    const endDayValue = $('#EndDay').val();

    if (startDayValue) {
        const castStartValue = new Date(startDayValue);
        const castendValue = new Date(endDayValue);

        const startDateValue = castStartValue.getFullYear() + '-' + ('00' + (castStartValue.getMonth() + 1)).slice(-2) + '-' + ('00' + castStartValue.getDate()).slice(-2);
        const startTimeValue = ('00' + castStartValue.getHours()).slice(-2) + ':' + ('00' + castStartValue.getMinutes()).slice(-2);
        const endDateValue = castendValue.getFullYear() + '-' + ('00' + (castendValue.getMonth() + 1)).slice(-2) + '-' + ('00' + castendValue.getDate()).slice(-2);
        const endTimeValue = ('00' + castendValue.getHours()).slice(-2) + ':' + ('00' + castendValue.getMinutes()).slice(-2);

        if (startTimeValue === "00:00") {
            $('#startDate').val(startDateValue);
            $('#endDate').val(endDateValue);
        } else {
            $('#startDate').val(startDateValue);
            $('#startTime').val(startTimeValue);

            $('#endDate').val(endDateValue);
            $('#endTime').val(endTimeValue);
        }
    }
}

//確認メッセージのはいいいえ
const checkButtonHtml = '<div class="d-flex justify-content-end">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">いいえ</a>' +
    '</div>' +
    '<div class="p-2">' +
    '<input type="submit" value="はい" class="btn btn-primary" />' +
    '</div>' +
    '</div>';

//もう一度入力のボタン
const yesButtonHtml = '<div class="p-2">' +
    '<input type="submit" value="もう一度入力" class="btn btn-primary" />' +
    '</div>';

//エラーメッセージの閉じるボタン
const errorCloseButton = '<div class="d-flex justify-content-end">' +
    '<div class="p-2">' +
    '<a asp-action="Index" class="btn btn-secondary" data-bs-dismiss="modal">閉じる</a>' +
    '</div>' +
    '</div>';

function showConfirmationModal(action, message, callback) {
    $('#messageModal').modal('show');
    $('#messageLavel').text('確認');
    $('#modalBody').text(message);
    $('#closeModalBtn').html(checkButtonHtml);

    $('#closeModalBtn').off('click').on('click', 'input[type="submit"]', function () {
        const checkedButtonValue = $(this).val();

        if (checkedButtonValue === "はい") {
            callback();
        } else if (checkedButtonValue === "いいえ") {
        }
        $('#messageModal').modal('hide');
    });
}

//エラーモーダル
function showErrorModal() {
    $('#messageModal').modal('show');
    $('#messageLavel').text('エラー');
    $('#modalBody').text('登録できませんでした。画面を確認してください。');
    $('#closeModalBtn').html(errorCloseButton);

    $('#closeModalBtn').on('click', 'input[type="submit"]', function () {
        const checkedButtonValue = $(this).val();

        if (checkedButtonValue === "閉じる") {
            $('#messageModal').modal('hide');
        }
    });
}

function scheduleErrorModal(message, callback) {
    $('#scheduleModal').modal('show');
    $('#scheduleLavel').text('確認');
    $('#scheduleBody').text(message);
    $('#scheduleBtn').html(checkButtonHtml);

    $('#scheduleBtn').on('click', 'input[type="submit"]', function () {
        const checkedButtonValue = $(this).val();
        if (checkedButtonValue === "はい") {
            $('#scheduleModal').modal('hide');
            callback();
        } else if (checkedButtonValue === "いいえ") {
            $('#scheduleModal').modal('hide');
        }
    });
}

function serverErrorModal(message) {
    $('#messageModal').modal('show');
    $('#messageLavel').text('入力チェック');
    $('#modalBody').text(message);
    $('#closeModalBtn').html(yesButtonHtml);

    $('#closeModalBtn').off('click').on('click', 'input[type="submit"]', function () {
        const checkedButtonValue = $(this).val();
        if (checkedButtonValue === "もう一度入力") {
            $('#messageModal').modal('hide');
        }
    });
}
