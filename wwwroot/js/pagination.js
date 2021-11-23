/*
Author: Szilveszter Dezsi
Created: 2020-01-24
Modified: 2020-04-24
*/

window.onload = filter_init;
const all_posts = document.getElementsByClassName('post');
const default_posts_per_page = 10;
const default_active_page_id = 1;

function filter_init() {
    filter(all_posts, default_posts_per_page, default_active_page_id);
}

function filter_request() {
    filter(all_posts, document.getElementById('ppp').value, default_active_page_id);
}

function filter(all_posts, posts_per_page, active_page_id) {
    var pagination = document.getElementById('pagination');
    set_visible_posts(all_posts, posts_per_page, active_page_id);
    if (all_posts.length <= default_posts_per_page) {
        pagination.style.display = "none";
    }
    else {
        pagination.style.display = "block";
        initialize_posts_per_page(all_posts.length, posts_per_page);
        initialize_pager(all_posts.length, posts_per_page, active_page_id);
    }
}

function set_visible_posts(posts, posts_per_page, active_page_id) {
    for (let i = 0; i < posts.length; i++) {
        if (i >= ((active_page_id - 1) * posts_per_page) && (i < (posts_per_page * active_page_id))) {
            posts[i].style.display = "grid";
        } else {
            posts[i].style.display = "none";
        }
    }
}

function initialize_posts_per_page(total_posts, posts_per_page) {
    var ppp_select = document.getElementById('ppp');
    while (ppp_select.firstChild) {
        ppp_select.removeChild(ppp_select.firstChild);
    }
    for (let i = default_posts_per_page; i < total_posts; i+=5) {
        ppp_select.appendChild(new Option(i, i));
    }
    ppp_select.appendChild(new Option("All", total_posts));
    ppp_select.value = posts_per_page;
}

function initialize_pager(total_posts, posts_per_page, active_page_id) {
    var pager = document.getElementById('pager');
    while (pager.firstChild) {
        pager.removeChild(pager.firstChild);
    }
    if (total_posts > posts_per_page) {
        var total_pages = Math.ceil(total_posts / posts_per_page);
        var page_links = `<a href="#" id="prev" class="disabled" onclick="prev_page()">&laquo</a>`;
        for (let i = 1; i <= total_pages; i++) {
            page_links += `<a href="#" id="${i}" onclick="active_page(this)">${i}</a>`;
        }
        page_links += `<a href="#" id="next" onclick="next_page()">&raquo</a>`;
        pager.innerHTML = page_links;
        pager.childNodes[active_page_id].className += "active";
    }
}

function active_page(new_active_page) {
    var old_active_page = document.getElementsByClassName('active');
    var posts_per_page = document.getElementById('ppp').value;
    var total_pages = Math.ceil(all_posts.length / posts_per_page);
    var new_active_page_id = new_active_page.id;
    var prev_page = document.getElementById('prev');
    var next_page = document.getElementById('next');
    old_active_page[0].className = old_active_page[0].className.replace("active", "");
    new_active_page.className += "active";
    set_visible_posts(all_posts, posts_per_page, new_active_page_id);
    prev_page.className = new_active_page_id == 1 ? prev_page.className += "disabled" : prev_page.className.replace("disabled", "");
    next_page.className = new_active_page_id == total_pages ? next_page.className += "disabled" : next.className.replace("disabled", "");
}

function next_page() {
    active_page(document.getElementsByClassName('active')[0].nextSibling);
}

function prev_page() {
    active_page(document.getElementsByClassName('active')[0].previousSibling);
}