$('.promo-text-anim').slideToggle(1500);
$('.how-look-list-item').on('click',function(event)
{
    $('.how-look-list-item').removeClass('how-look-active');
    event.target.classList.add('how-look-active');
    $('.how-look-img').attr('src','../Content/HowLook/img/'+event.target.value+'.svg');
})
$(window).scroll(function() {
    if ($(window).width() >= 549) {
        if($(window).scrollTop() >= 200) {
            // анимация, которая должны быть выполнена
            slipAnimation(1);
        }
        if($(window).scrollTop()>=600){
            slipAnimation(2);
        }
    }
});

if($(window).scrollTop() >= 0) {
    animation();
    slipAnimation(0);
}

function animation()
{
    $('.capa-list:eq(0)').delay(100).animate({
        opacity: 1,
    }, 'slow');
    $('.capa-list:eq(1)').delay(500).animate({
        opacity: 1,
    }, 'slow');
    $('.capa-list:eq(2)').delay(1000).animate({
        opacity: 1,
    }, 'slow');
}
function slipAnimation(num)
{
    $('.img-capa:eq('+num+')').delay(200).animate({
        opacity: 1,
        right:0,
    }, 'slow');
}