mergeInto(LibraryManager.library, {
	SyncFS: function () {
		FS.syncfs(false, function (err) {
			if (err) {
				console.error("Failed to sync filesystem:", err);
			}
		});
	}
});