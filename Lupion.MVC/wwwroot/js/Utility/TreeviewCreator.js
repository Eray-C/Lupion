function renderTreeview(
    treeId,
    dataSource,
    itemClickFn = null,
    settings = {},
    dataStructure = 'plain',
    enableSearch = false
) {
    const treeSettings = {
        dataSource: dataSource,
        dataStructure: dataStructure,
        keyExpr: 'id',
        displayExpr: 'text',
        selectionMode: 'single',
        searchEnabled : enableSearch,
        paging: {
            enabled: true,
            pageSize: 15,
            pageIndex: 1   
        },
        onItemClick: function (e) {
            if (itemClickFn) itemClickFn(e.itemData);
        },
        ...settings
    };

    const treeContainer = $(`#${treeId}`);

    const treeInstance = treeContainer.dxTreeView(treeSettings).dxTreeView('instance');

    return treeInstance;
}



function renderTreeDynamic(treeId, options) {

    const {
        url,
        rootValue = null,
        searchEnabled = true,
        pageSize = 50,
        getExtraParams = null,
        onItemClickFn = null,
        onSearchChangedFn = null
    } = options;

    let treeSearchValue = "";

    const treeInstance = $(`#${treeId}`).dxTreeView({
        dataStructure: "plain",
        rootValue: rootValue,
        virtualModeEnabled: true,
        searchEnabled: searchEnabled,
        searchMode: "contains",
        searchValue: treeSearchValue,

        createChildren(parent) {
            const parentId = parent ? parent.itemData.id : null;

            const baseParams = {
                parentId: parentId ?? "",
                search: treeSearchValue ?? "",
                skip: 0,
                take: pageSize
            };

            const extraParams = getExtraParams ? getExtraParams(parent) : {};
            const params = new URLSearchParams({ ...baseParams, ...extraParams });

            return new Promise((resolve, reject) => {
                genericAjax({
                    url: `${url}?${params}`,
                    method: "GET",
                    success: function (response) {
                        const data = response.data ?? response;
                        resolve(
                            data.map(n => ({
                                id: n.id,
                                parentId: n.parentId,
                                text: n.text,
                                hasItems: n.hasChildren,
                                nodeType: n.nodeType
                            }))
                        );
                    },
                    error: reject
                });
            });
        },

        onItemClick(e) {
            if (onItemClickFn) onItemClickFn(e.itemData, e.component);
        },

        onOptionChanged(e) {
            if (e.name === "searchValue") {
                treeSearchValue = e.value || "";
                if (onSearchChangedFn) onSearchChangedFn(treeSearchValue);
                e.component.dispose();
                renderTreeDynamic(treeId, options);
            }
        }

    }).dxTreeView("instance");

    return treeInstance;
}
