// This is a customized/improved version of jquery.rowselector.js:
// * Disable text selection only while the shift-key is pressed to select multiple rows.
// * Support the use of the ctrl.-key to select rows one by one.
// * Unselect multiple rows while holding the shift-key, rather than only the last row.
// * Unselect all other rows while not using the ctrl./shift-key.
// * Introduce ctrl+A to select all rows.

(function ($)
{
	$.fn.selectedrows = function ()
	{
		var table = $(this[0]);
		var match = undefined;

		if ((table.prop('tagName').toUpperCase() === 'TABLE') && (typeof table.attr('data-rs-selectable') !== 'undefined'))
		{
			var c = table.attr('data-rs-class') || 'selected';
			match = $(table).find('tbody tr.' + c);
		}

		return match;
	};
})(jQuery);


$(document).ready(function ()
{
	var tables = {};

	//MOD.os
	// $('body').on('mouseover', 'table[data-rs-selectable]', function (evt)
	// {
	// 	$(this).addClass('unselectable').attr('unselectable', 'on');
	// });
	//MOD.oe

	//MOD.ns
	// Prevent text select from shift + click
	$('body').on('mousedown', 'table[data-rs-selectable]', function(evt)
	{
		if (evt.shiftKey || evt.ctrlKey) {
			evt.preventDefault();
		}
	});

	
	// Select all rows on ctrl + A
	$('body').on('keydown', 'table[data-rs-selectable] tr', function(evt)
	{
		if (!evt.ctrlKey) {
			return;
		}
		
		if (evt.keyCode !== 65) {
			return;
		}

		var $this = $(this);
		var table = $this.closest('table');

		var t = table.attr('data-rs-type')  || 'many';
		var c = table.attr('data-rs-class') || 'selected';
		
		if (t !== 'none' && t !== 'one') {
			$(this).addClass(c);
			$(this).siblings().addClass(c);
		}
		
		tables[table.id] = this;
		$(table).trigger("clicked.rs.row");
	});
	//MOD.ne

	$('body').on('click', 'table[data-rs-selectable] tr', function (evt)
	{
		var $this = $(this);
		var table = $this.closest('table');

		var t = table.attr('data-rs-type')  || 'many';
		var c = table.attr('data-rs-class') || 'selected';

		if (t !== 'none')
		{
			if (t === 'one')
			{
				$(this).siblings().removeClass(c).end().addClass(c);
			}
			else
			{
				$(this).toggleClass(c);

				if (evt.shiftKey)
				{
					var last = tables[table.id] || false;
					// if ((last) && (this !== last) && ($(this).hasClass(c) === $(last).hasClass(c)))	//MOD.o
					if ((last) && (this !== last))														//MOD.n
					{
						var startat = (this.rowIndex > last.rowIndex) ? last : this;
						var endat   = (this.rowIndex > last.rowIndex) ? this : last;
						var do_add  = $(this).hasClass(c);
						//MOD.ns
						if (do_add) {
							$(last).addClass(c);
						}
						else {
							$(last).removeClass(c);
						}
						//MOD.ne

						var rows = $(startat).nextAll('tr');
						for (var i = 0, l = rows.length; i < l; i += 1)
						{
							if (rows[i] === endat) {break;}
							if (do_add)
							{
								$(rows[i]).addClass(c);
							}
							else
							{
								$(rows[i]).removeClass(c);
							}
						}
					}
				}
				//MOD.ns
				else if (!evt.ctrlKey)
				{
					$(this).siblings().removeClass(c).end().addClass(c);
				}
				//MOD.ne
			}
		}

		tables[table.id] = this;
		$(table).trigger("clicked.rs.row");
	});
});
