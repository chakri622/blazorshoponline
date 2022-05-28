function MakeUpdateQtyButtonVisible(id, visible) {
	const updateQtyButton = document.querySelector("button[data-itemid=\"" + id + "\"]");
	if (visible) {
		updateQtyButton.style.display = "inline-block";
	} else {
		pdateQtyButton.style.display = "none";
	}
}