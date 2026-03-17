let archiveMode = false;
let searchDebounceHandle = null;
const filterState = {
    search: "",
    statuses: [],
    priorities: []
};

$(document).ready(function () {
    bindSearch();
    bindFilterDrawer();
    refreshKanban();
});

function refreshKanban() {

    const queryString = buildTaskQuery(archiveMode);
    const $btn = $("#btnArchive");
    const $icon = $btn.find("i");
    const $text = $btn.find(".btn-text");

    if (archiveMode) {

        $("#kanbanBoard").hide();
        $("#kanbanGridContainer").show();

        createArchiveGrid();
        loadArchiveTasks(queryString);

        $icon
            .removeClass("fa-box-archive")
            .addClass("fa-border-all");
        $text.text("Çalışma Tahtası");

    } else {

        const url = queryString
            ? `/task-manager/board?${queryString}`
            : "/task-manager/board";

        $("#kanbanGridContainer").hide();
        $("#kanbanBoard").show().load(url, function () {
            initializeDragAndDrop();
        });

        $icon
            .removeClass("fa-border-all")
            .addClass("fa-box-archive");
        $text.text("Arşiv");
    }
}

function toggleArchiveMode() {
    archiveMode = !archiveMode;
    refreshKanban();
}

function bindSearch() {
    $("#txtSearch").on("input", function () {
        filterState.search = $(this).val();

        if (searchDebounceHandle) {
            clearTimeout(searchDebounceHandle);
        }

        searchDebounceHandle = setTimeout(function () {
            refreshKanban();
        }, 300);
    });
}

function createArchiveGrid() {

    const columns = [
        { dataField: "taskNumber", caption: "İş Numara" },
        { dataField: "description", caption: "Açıklama" },
        { dataField: "statusName", caption: "Durum" },
        { dataField: "priorityName", caption: "Öncelik" },
        { dataField: "taskDate", caption: "İş Tarihi", dataType: "date", format: "dd.MM.yyyy" }
    ];
    const contextMenu = function (rowData) {
        return [
            {
                text: "Ekle",
                icon: "plus",
                onItemClick: function () {
                    openAddModal();
                }
            },
            {
                text: "Düzenle",
                icon: "edit",
                onItemClick: function () {
                    openEditModal(rowData.id);
                }
            },
            {
                text: "Sil",
                icon: "trash",
                onItemClick: function () {
                    if (!rowData) return;
                    deleteTask(rowData.id)
                }
            },
            {
                text: "Dosyalar",
                icon: "doc",
                onItemClick: function () {
                    if (!rowData) return
                    openAttachmentModal("GeneralTaskAttachments", rowData.id)
                }
            },
            {
                text: "Arşivden Geri Getir",
                icon: "arrowleft",
                onItemClick: function () {
                    if (!rowData) return;
                    unarchiveTask(rowData.id);
                }
            }
        ];
    };

    renderDatagrid('task_archive_grid', null, columns, null, contextMenu);
}

function bindFilterDrawer() {
    $("#btnFilter").on("click", function () {
        toggleFilterDrawer();
        if ($("#filterDrawer").hasClass("drawer-open")) {
            const activeType = $(".filter-category.active").data("type") || "status";
            setActiveFilterCategory(activeType);
        }
    });

    $(".filter-category").on("click", function () {
        setActiveFilterCategory($(this).data("type"));
    });

    $(".filter-clear").on("click", function () {
        filterState.statuses = [];
        filterState.priorities = [];
        updateFilterBadges();
        renderFilterOptions($(".filter-category.active").data("type") || "status");
        refreshKanban();
    });

    const defaultType = $(".filter-category").first().data("type") || "status";
    setActiveFilterCategory(defaultType);
    updateFilterBadges();
}

function toggleFilterDrawer() {
    const $drawer = $("#filterDrawer");
    const isOpen = $drawer.hasClass("drawer-open");

    if (isOpen) {
        closeFilterDrawer();
    } else {
        openFilterDrawer();
    }
}

function openFilterDrawer() {
    const $drawer = $("#filterDrawer");

    $drawer.removeClass("d-none").addClass("drawer-open");

    setTimeout(() => {
        $(document).on("click.filterDrawer", function (e) {
            if (!$(e.target).closest("#filterDrawer, #btnFilter").length) {
                closeFilterDrawer();
            }
        });
    }, 0);
}

function closeFilterDrawer() {
    const $drawer = $("#filterDrawer");

    $drawer.removeClass("drawer-open").addClass("d-none");

    $(document).off("click.filterDrawer");
}


function setActiveFilterCategory(type) {
    $(".filter-category").removeClass("active");
    $(`.filter-category[data-type='${type}']`).addClass("active");
    renderFilterOptions(type);
}

function renderFilterOptions(type) {
    const options = getLookupOptions(type);
    const selected = type === "status" ? filterState.statuses : filterState.priorities;

    if (!options.length) {
        $("#drawerOptions").text("Filtre oluşturmak için seçenek bulunamadı.");
        return;
    }

    const optionItems = options.map((option) => {
        const optionId = Number(option.id ?? option.Id);
        const optionName = option.name ?? option.Name ?? "";
        const isChecked = selected.includes(optionId);

        return `
    <div class="filter-option-card ${isChecked ? "active" : ""}">
        <label class="d-flex align-items-center gap-2 m-0 w-100">
            <input 
                type="checkbox"
                class="form-check-input filter-option"
                data-type="${type}"
                value="${optionId}"
                ${isChecked ? "checked" : ""}
            />
            <span class="filter-dot" style="background:${option.color ?? '#3B82F6'}"></span>
            <span class="filter-text">${optionName}</span>
        </label>
    </div>
`;

    }).join("");

    $("#drawerOptions").html(optionItems);
    $(".filter-option").on("change", handleFilterSelection);
}

function handleFilterSelection() {
    const type = $(this).data("type");
    const value = Number($(this).val());
    const target = type === "status" ? filterState.statuses : filterState.priorities;
    const valueIndex = target.indexOf(value);

    if ($(this).is(":checked")) {
        if (valueIndex === -1) {
            target.push(value);
        }
    } else if (valueIndex !== -1) {
        target.splice(valueIndex, 1);
    }

    updateFilterBadges();
    refreshKanban();
}

function updateFilterBadges() {
    $(".filter-category").each(function () {
        const type = $(this).data("type");
        const count = type === "status" ? filterState.statuses.length : filterState.priorities.length;
        $(this).find(".fc-badge").text(count > 0 ? count : "");
    });

    const total = filterState.statuses.length + filterState.priorities.length;
    $("#filterCount").text(total);
}

function getLookupOptions(type) {
    const lookups = window.taskManagerLookups || {};
    return type === "status" ? (lookups.statuses || []) : (lookups.priorities || []);
}

function buildTaskQuery(includeArchiveFlag = false) {
    const params = new URLSearchParams();

    if (includeArchiveFlag) {
        params.append("archive", "true");
    }

    const searchTerm = filterState.search?.trim();
    if (searchTerm) {
        params.append("search", searchTerm);
    }

    filterState.statuses.forEach((statusId) => params.append("statusIds", statusId));
    filterState.priorities.forEach((priorityId) => params.append("priorityIds", priorityId));

    return params.toString();
}

function openAddModal() {
    openTaskModal();
}

function openEditModal(id) {
    openTaskModal(id);
}

function openTaskModal(id) {
    showModal({
        url: '/task-manager/add-edit/',
        title: id ? 'İş Düzenle' : 'İş Ekle',
        size: 'xl',
        data: id ? { id: id } : {},
        successEvent: function () {
            if (!id) {
                fetchNextTaskNumber();
            }
        },
    });
}

function fetchNextTaskNumber() {
    genericAjax({
        url: 'task-manager/tasks/next-number',
        isAuth: true,
        success: function (res) {
            if (!$('#TaskNumber').val()) {
                $('#TaskNumber').val(res.message);
            }
        }
    });
}

function submitTask() {
    const formData = serializeForm("addEditTechTaskForm");
    const hasId = !!formData.Id;
    const method = hasId ? "PUT" : "POST";
    const url = hasId
        ? `task-manager/tasks/${formData.Id}`
        : `task-manager/tasks`;

    genericAjax({
        url: url,
        method: method,
        data: formData,
        isAuth: true,
        success: function () {
            refreshKanban();
        }
    })
}

function loadArchiveTasks(queryString) {
    const url = queryString ? `task-manager/tasks?${queryString}` : 'task-manager/tasks';

    genericAjax({
        url: url,
        isAuth: true,
        success: function (res) {
            $("#task_archive_grid").dxDataGrid("instance").option("dataSource", res.data);
        }
    })
}

function deleteTask(id) {
    showDeleteAlert(`task-manager/tasks/${id}`, () => refreshKanban());
}

function archiveTask(id) {
    genericAjax({
        url: `task-manager/tasks/${id}/archive`,
        method: "POST",
        isAuth: true,
        success: function () {
            refreshKanban();
        }
    });
}

function unarchiveTask(id) {
    genericAjax({
        url: `task-manager/tasks/${id}/unarchive`,
        method: "POST",
        isAuth: true,
        success: function () {
            loadArchiveTasks(buildTaskQuery(true));
        }
    });
}

function initializeDragAndDrop() {
    const dragState = {
        taskId: null,
        fromStatus: null
    };

    const $board = $("#kanbanBoard");
    $board.find(".kanban-item").attr("draggable", "true");

    $board.off("dragstart", ".kanban-item");
    $board.off("dragend", ".kanban-item");
    $board.off("dragover", ".kanban-items");
    $board.off("dragleave", ".kanban-items");
    $board.off("drop", ".kanban-items");

    $board.on("dragstart", ".kanban-item", function (event) {
        dragState.taskId = $(this).data("task-id");
        dragState.fromStatus = $(this).data("status");

        const transfer = event.originalEvent.dataTransfer;
        transfer.effectAllowed = "move";
        transfer.setData("text/plain", dragState.taskId);
    });

    $board.on("dragend", ".kanban-item", function () {
        dragState.taskId = null;
        dragState.fromStatus = null;
        $board.find(".kanban-items").removeClass("drag-over");
    });

    $board.on("dragover", ".kanban-items", function (event) {
        event.preventDefault();
        event.originalEvent.dataTransfer.dropEffect = "move";
        $(this).addClass("drag-over");
    });

    $board.on("dragleave", ".kanban-items", function () {
        $(this).removeClass("drag-over");
    });

    $board.on("drop", ".kanban-items", function (event) {
        event.preventDefault();
        const targetStatus = $(this).data("status-id");
        const taskId = event.originalEvent.dataTransfer.getData("text/plain");
        $(this).removeClass("drag-over");

        if (!taskId || targetStatus === dragState.fromStatus) {
            return;
        }

        updateTaskStatus(taskId, dragState.fromStatus, targetStatus);
    });
}

function updateTaskStatus(taskId, oldStatusId, newStatusId) {
    genericAjax({
        url: `task-manager/tasks/${taskId}/status`,
        method: "POST",
        data: {
            TaskId: taskId,
            OldStatusId: oldStatusId,
            NewStatusId: newStatusId,
        },
        isAuth: true,
        success: function () {
            refreshKanban();
        },
    });
}
