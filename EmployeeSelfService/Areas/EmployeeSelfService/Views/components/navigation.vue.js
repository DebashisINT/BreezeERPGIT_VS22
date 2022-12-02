

Vue.component('rating-counter', {
    props: ['title', 'parent'],
    data() {
        return {
            count: 0
        }
    },
    template:   `<div>
                    <h1>{{ title }}</h1>
                    <button v-on:click="count--;parent.globalCount--;">Thumbs Down</button>
                    {{ count }}
                    <button v-on:click="count++;parent.globalCount++;">Thumbs Up</button>
                </div>`
})

